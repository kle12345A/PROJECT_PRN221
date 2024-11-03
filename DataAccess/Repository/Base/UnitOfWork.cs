using DataAccess.Models;
using DataAccess.Repository.banner;
using DataAccess.Repository.category;
using DataAccess.Repository.contact;
using DataAccess.Repository.news;
using DataAccess.Repository.order;
using DataAccess.Repository.product;
using DataAccess.Repository.user;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repository.Base
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PROJECT_PRN212Context _context;

        public IProductRepository Products { get; }
        public IBannerRepository Banners { get; }
        public INewsRepository News { get; }
        public IContactRepository Contacts { get; }
        public ICategoryRepository Categories { get; }
        public IUserRepository Users { get; }

		public IOrdersRepository Orders { get; }


		public UnitOfWork(
            PROJECT_PRN212Context context,
            IProductRepository productRepository,
            IBannerRepository bannerRepository,
            INewsRepository newsRepository,
            IContactRepository contactRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IOrdersRepository ordersRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Products = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            Banners = bannerRepository ?? throw new ArgumentNullException(nameof(bannerRepository));
            News = newsRepository ?? throw new ArgumentNullException(nameof(newsRepository));
            Contacts = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            Categories = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            Users = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            Orders = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));

		}

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class
        {
            return new BaseRepository<TEntity>(_context);
        }

    }
}
