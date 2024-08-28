    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer
{

    public interface ICodingSessionRepository
    {
        Task<CodingSession> GetByUserIdAsync(int id);
        Task<CodingSession> GetByCodingSessionIdAsync(int id);
        Task<int> AddCodingSessionAsync(CodingSession codingSession);
        Task<int> UpdateCodingSessionAsync(CodingSession codingSession);
        Task<int> DeleteCodingSessionAsync(int id);
        Task<IEnumerable<CodingSession>> GetAllAsync();


    }
}
