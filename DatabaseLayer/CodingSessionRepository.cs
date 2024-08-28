using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace DatabaseLayer

{
    public class CodingSessionRepository : ICodingSessionRepository

    {
        DatabaseConfiguration _dbconfig;
        public CodingSessionRepository(DatabaseConfiguration databaseConfiguration)
        {
            _dbconfig = databaseConfiguration;
        }

        public async Task<int> AddCodingSessionAsync(CodingSession codingSession)
        {
            if (codingSession == null)
            {
                throw new ArgumentNullException(nameof(codingSession));
            }
            using (var connection = _dbconfig.CreateConnection())
            {
                await connection.OpenAsync();

                var sql = "INSERT INTO public.\"CODING_SESSION\" (user_id, start_time, end_time, session_duration) " +
                            "VALUES (@UserId, @StartTime, @EndTime, @SessionDuration) RETURNING coding_session_id;";
                var id = await connection.ExecuteScalarAsync<int>(sql, codingSession);
                return id;

            }
        }

        public async Task<int> DeleteCodingSessionAsync(int id)
        {
            using (var connection = _dbconfig.CreateConnection())
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM public.\"CODING_SESSION\" WHERE coding_session_id = @CodingSessionId";
                var affectedRows = await connection.ExecuteAsync(sql, new { CodingSessionId = id });
                return affectedRows;
            }
        }

        public async Task<IEnumerable<CodingSession>> GetAllAsync()
        {
            using (var connection = _dbconfig.CreateConnection())
            {
                await connection.OpenAsync();
                var sql = "SELECT * FROM public.\"CODING_SESSION\"";
                var codingSessions = await connection.QueryAsync<CodingSession>(sql);
                return codingSessions;
            }

        }

        public async Task<CodingSession> GetByCodingSessionIdAsync(int id)
        {
            using (var connection = _dbconfig.CreateConnection())
            {
                await connection.OpenAsync();
                var sql = "SELECT * FROM public.\"CODING_SESSION\" WHERE coding_session_id = @CodingSessionId";
                var codingSession = await connection.QueryFirstOrDefaultAsync<CodingSession>(sql, new { CodingSessionId = id });
                return codingSession;
            }
        }    public async Task<CodingSession> GetByUserIdAsync(int id)
        {
            using (var connection = _dbconfig.CreateConnection())
            {
                await connection.OpenAsync();
                var sql = "SELECT * FROM public.\"CODING_SESSION\" WHERE user_id = @UserId";
                var codingSession = await connection.QueryFirstOrDefaultAsync<CodingSession>(sql, new { UserId = id });
                return codingSession;
            }
        }

        public async Task<int> UpdateCodingSessionAsync(CodingSession codingSession)
        {
            if (codingSession == null)
            {
                throw new ArgumentNullException(nameof(codingSession));
            }
            using (var connection = _dbconfig.CreateConnection())
            {
                await connection.OpenAsync();
                var sql = "UPDATE  public.\"CODING_SESSION\" SET user_id = @UserId," +
                    "start_time = @StartTime, end_time = @EndTime, session_duration = @SessionDuration WHERE coding_session_id = @CodingSessionId;";
                var affectedRows = await connection.ExecuteAsync(sql, codingSession);
                return affectedRows;

            }
        }
    }
}
