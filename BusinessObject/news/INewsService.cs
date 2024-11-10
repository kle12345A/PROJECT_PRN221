using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.news
{
    public interface INewsService : IBaseService<News>
    {
        Task<IEnumerable<News>> SearchAsync(string searchTerm);
    }
}
