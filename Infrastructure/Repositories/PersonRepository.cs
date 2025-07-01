using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.Repositories
{
    public class PersonRepository : IRepository<Person>
    {
        private readonly string _connectionString;

        public PersonRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> AddAsync(Person entity)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Person (Name, Cpf, DateOfBirth) 
                       VALUES (@Name, @Cpf, @DateOfBirth);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await db.QuerySingleAsync<int>(sql, entity);
            return id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE Person WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            return affectedRows > 0;
        }

        public Task<Person> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Person entity)
        {
            var sql = @"
                UPDATE Person 
                SET Name = @Name, 
                    Cpf = @Cpf,
                    DateOfBirth = @DateOfBirth
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, entity);

            return affectedRows > 0;
        }
    }
}
