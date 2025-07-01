using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly string _connectionString;

        private class InsuredMap
        {
            public int Insured_Id { get; set; }
            public string Insured_Name { get; set; }
            public string Insured_Cpf { get; set; }
            public DateTime Insured_DateOfBirth { get; set; }
        }

        private class VehicleMap
        {
            public int Vehicle_Id { get; set; }
            public decimal Vehicle_Value { get; set; }
            public string Vehicle_Branch { get; set; }
            public string Vehicle_Model { get; set; }
        }

        public ClaimRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> AddAsync(Claim entity)
        {
            entity.CreateDate = DateTime.UtcNow;

            using var db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Claim (CreateDate, InsuredId, VehicleId, ClaimValue) 
                       VALUES (@CreateDate, @InsuredId, @VehicleId, @ClaimValue);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await db.QuerySingleAsync<int>(sql, entity);
            return id;
        }


        public async Task<Claim> GetById(int id)
        {
            var sql = @"
                SELECT Claim.*, 
                       Insured.Id as Insured_Id, 
                       Insured.Name as Insured_Name,
                       Insured.Cpf as Insured_Cpf,
                       Insured.DateOfBirth as Insured_DateOfBirth,
                       Vehicle.Id as Vehicle_Id,
                       Vehicle.Value as Vehicle_Value,
                       Vehicle.Branch as Vehicle_Branch,
                       Vehicle.Model as Vehicle_Model

                FROM Claim Claim
                INNER JOIN Person Insured ON Claim.InsuredId = Insured.Id
                INNER JOIN Vehicle Vehicle ON Claim.VehicleId = Vehicle.Id
                WHERE Claim.Id = @Id ";

            using var connection = new SqlConnection(_connectionString);
            var claimMap = await connection.QueryAsync<Claim, InsuredMap, VehicleMap, Claim>(
                sql,
                (claim, insured, vehicle) =>
                {
                    claim.Insured = new Person(insured.Insured_Id, insured.Insured_Name, insured.Insured_Cpf, insured.Insured_DateOfBirth);
                    claim.Vehicle = new Vehicle(vehicle.Vehicle_Id, vehicle.Vehicle_Value, vehicle.Vehicle_Branch, vehicle.Vehicle_Model);

                    return claim;
                },
                new { Id = id },
                splitOn: "Insured_Id,Vehicle_Id"
            );

            return claimMap.FirstOrDefault();
        }

        public async Task<IEnumerable<Claim>> GetAllClaims(string? InsuredName = "", string? InsuredCpf = "", string? Branch = "", string? Model = "")
        {
            var sql = @"
                SELECT Claim.*, 
                       Insured.Id as Insured_Id, 
                       Insured.Name as Insured_Name,
                       Insured.Cpf as Insured_Cpf,
                       Insured.DateOfBirth as Insured_DateOfBirth,
                       Vehicle.Id as Vehicle_Id,
                       Vehicle.Value as Vehicle_Value,
                       Vehicle.Branch as Vehicle_Branch,
                       Vehicle.Model as Vehicle_Model

                FROM Claim Claim
                INNER JOIN Person Insured ON Claim.InsuredId = Insured.Id
                INNER JOIN Vehicle Vehicle ON Claim.VehicleId = Vehicle.Id 
                WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(InsuredName))
                sql += " AND Insured.Name = @InsuredName ";

            if (!string.IsNullOrEmpty(InsuredCpf))
                sql += " AND Insured.Cpf = @InsuredCpf ";

            if (!string.IsNullOrEmpty(Branch))
                sql += " AND Vehicle.Branch = @Branch ";

            if (!string.IsNullOrEmpty(Model))
                sql += " AND Vehicle.Model = @Model ";

            var parameters = new { InsuredName, InsuredCpf, Branch, Model };

            using var connection = new SqlConnection(_connectionString);
            var claimMap = await connection.QueryAsync<Claim, InsuredMap, VehicleMap, Claim>(
                sql,
                (claim, insured, vehicle) =>
                {
                    claim.Insured = new Person(insured.Insured_Id, insured.Insured_Name, insured.Insured_Cpf, insured.Insured_DateOfBirth);
                    claim.Vehicle = new Vehicle(vehicle.Vehicle_Id, vehicle.Vehicle_Value, vehicle.Vehicle_Branch, vehicle.Vehicle_Model);
                    return claim;
                },
                parameters,
                splitOn: "Insured_Id,Vehicle_Id"
            );

            return claimMap;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Claim WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            
            return affectedRows > 0;
        }

        public async Task<bool> UpdateAsync(Claim entity)
        {
            var sql = @"
                UPDATE Claim 
                SET UpdateDate = @UpdateDate, ClaimValue = @ClaimValue
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, entity);
            
            return affectedRows > 0;
        }
    }
}
