using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class VehicleRepository : IRepository<Vehicle>
    {
        private readonly string _connectionString;

        public VehicleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> AddAsync(Vehicle entity)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Vehicle (Value, Branch, Model) 
                       VALUES (@Value, @Branch, @Model);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await db.QuerySingleAsync<int>(sql, entity);
            return id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE Vehicle WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            return affectedRows > 0;
        }

        public Task<Vehicle> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Vehicle entity)
        {
            var sql = @"
                UPDATE Vehicle 
                SET Value = @Value,
                Branch = @Branch,
                Model = @Model
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, entity);

            return affectedRows > 0;
        }
    }
}
