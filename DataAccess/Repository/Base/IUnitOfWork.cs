using DataAccess.Repository.banner;
using DataAccess.Repository.category;
using DataAccess.Repository.contact;
using DataAccess.Repository.news;
using DataAccess.Repository.product;
using DataAccess.Repository.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repository.order;

namespace DataAccess.Repository.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IBannerRepository Banners { get; }
        INewsRepository News { get; }
        IContactRepository Contacts { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
		IOrdersRepository Orders { get; }


		Task SaveChangesAsync();
        IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class; 
    }
}
