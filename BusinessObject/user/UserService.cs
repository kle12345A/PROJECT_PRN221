using DataAccess.Models;
using DataAccess.Repository.Base;
using DataAccess.Repository.user;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Kiểm tra xem người dùng đã tồn tại trong database hay chưa 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        public User? Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

           
            var user = _userRepository.GetAll()
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            return user;
        }



        /// <summary>
        /// Lấy ra tất cả người dùng theo role của họ
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IList<User>> GetAllWithRolesAsync()
        {
         
            if (_userRepository == null)
            {
                throw new InvalidOperationException("User repository is not initialized.");
            }
            var usersWithRoles = _userRepository.GetAll().Include(u => u.Role); 
            return await usersWithRoles.ToListAsync();
        }

        /// <summary>
        /// Lấy ra thông tin người dùng theo địa chỉ email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

          
            return await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Email == email);

        }
        /// <summary>
        /// Đếm tổng số lượng user
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _unitOfWork.BaseRepository<User>().GetQuery().CountAsync();
        }


        /// <summary>
        /// Cập nhật lại thời gian login cuối cùng vào hệ thống
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateLastLoginAsync(User user)
        {
            

            
            user.LastLogin = DateTime.Now;
           await _unitOfWork.BaseRepository<User>().UpdateAsync(user);
          
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật lại mật khẩu 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePasswordAsync(User user, string newPassword)
        {
            user.Password = newPassword;

             await _unitOfWork.BaseRepository<User>().UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(); 

            return true; 
        }


    }
}
