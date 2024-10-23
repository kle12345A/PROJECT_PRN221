using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.user
{
    public interface IUserService : IBaseService<User>
    {
        Task<IList<User>> GetAllWithRolesAsync();
        User? Authenticate(string email, string password);
    }
}
