using DataAccess.Models;
using DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.news
{
    public class NewsService : BaseService<News>, INewsService
    {
        public NewsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
