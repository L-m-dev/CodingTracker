using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace DatabaseLayer
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        DatabaseConfiguration _dbConfig;

        public ApplicationUserRepository(DatabaseConfiguration dbConfig)
        {
            _dbConfig = dbConfig;
        } 

        public async Task<int> AddApplicationUser(ApplicationUserED applicationUser)
        {
            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser));
            }
            using (var connection = _dbConfig.CreateConnection())
            {
                await connection.OpenAsync();

                var sql = "INSERT INTO public.\"APPLICATION_USER\" (user_id, user_name) " +
                            "VALUES (@UserId, @UserName) RETURNING user_id;";
                var id = await connection.ExecuteScalarAsync<int>(sql, applicationUser);
                return id;

            }
        }

        public async Task<ApplicationUserED> GetByUserId(int userId)
        {
            using (var connection = _dbConfig.CreateConnection())
            {
                await connection.OpenAsync();
                var sql = "SELECT * FROM public.\"APPLICATION_USER\" WHERE user_id = @UserId";
                var applicationUser = await connection.QueryFirstOrDefaultAsync<ApplicationUserED>(sql, new { UserId = userId });
                return applicationUser;
            }
        }
    }
}
