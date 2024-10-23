using DataAccess.Models;
using DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.product
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(PROJECT_PRN212Context context) : base(context)
        {
        }
    }
}
