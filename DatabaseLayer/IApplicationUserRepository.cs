using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUserED> GetByUserId(int userId);
        Task<int> AddApplicationUser(ApplicationUserED applicationUser);
    }
}
