using DataAccess.Models;
using DataAccess.Repository.Base;
using System.Linq;

namespace DataAccess.Repository.user
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(PROJECT_PRN212Context context) : base(context)
        {
        }

        public IQueryable<User> GetAll()
        {
            return _dbSet.AsQueryable(); 
        }
    }
}
