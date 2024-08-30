    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer
{

    public interface ICodingSessionRepository
    {
        Task<IEnumerable<CodingSessionED>> GetByUserIdAsync(int id);
        Task<CodingSessionED> GetByCodingSessionIdAsync(int id);
        Task<int> AddCodingSessionAsync(CodingSessionED codingSession);
        Task<int> UpdateCodingSessionAsync(CodingSessionED codingSession);
        Task<int> DeleteCodingSessionAsync(int id);
        Task<IEnumerable<CodingSessionED>> GetAllAsync();


    }
}
