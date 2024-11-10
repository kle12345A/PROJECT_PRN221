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
        Task<int> GetTotalUsersCountAsync();
        Task<User?> GetByEmailAsync(string email);
        Task<bool> UpdatePasswordAsync(User user, string newPassword);
        Task UpdateLastLoginAsync(User user);
    }
}
