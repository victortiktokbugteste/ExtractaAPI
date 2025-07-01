using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Application.Interfaces;
using Application.Dtos;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class LogService : ILogService
    {
        private readonly string _connectionString;

        public LogService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
        }

        public async Task RegistrarAsync(LogDto logDto)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO ApplicationMiddlewareLogError (CreateDate, Method, Exception, Trace, StatusCode) 
                       VALUES (@CreateDate, @Method, @Exception, @Trace, @StatusCode);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            var log = new ApplicationMiddlewareLogError
            {
                CreateDate = logDto.CreateDate,
                Method = logDto.Method,
                Exception = logDto.Exception,
                Trace = logDto.Trace,
                StatusCode = logDto.StatusCode
            };

            await db.ExecuteAsync(sql, log);
        }
    }
}
