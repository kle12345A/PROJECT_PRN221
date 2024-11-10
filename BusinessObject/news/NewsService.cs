using DataAccess.Models;
using DataAccess.Repository.Base;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<News>> SearchAsync(string searchTerm) 
        {
            return await _unitOfWork.BaseRepository<News>().GetQuery()
                .Where(n => n.Title.Contains(searchTerm) || n.Description.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
