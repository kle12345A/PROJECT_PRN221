using DataAccess.Models;
using DataAccess.Repository.Base;
using DataAccess.Repository.user;
using Microsoft.EntityFrameworkCore; // Đảm bảo đã thêm namespace này
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessObject.user
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userRepository = unitOfWork.Users;
        }

        public User? Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

           
            var user = _userRepository.GetAll()
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            return user;
        }

        public async Task<IList<User>> GetAllWithRolesAsync()
        {
         
            if (_userRepository == null)
            {
                throw new InvalidOperationException("User repository is not initialized.");
            }

            // Lấy IQueryable<User> và bao gồm Role
            var usersWithRoles = _userRepository.GetAll().Include(u => u.Role); 
            return await usersWithRoles.ToListAsync();
        }
    }
}
