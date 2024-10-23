using DataAccess.Models;
using DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.news
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(PROJECT_PRN212Context context) : base(context)
        {
        }

        
    }
}
