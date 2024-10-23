using DataAccess.Models;
using DataAccess.Repository.banner;
using DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.category
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(PROJECT_PRN212Context context) : base(context)
        {

        }
    }
}
