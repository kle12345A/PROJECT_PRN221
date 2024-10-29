using DataAccess.Models;
using DataAccess.Repository.Base;
using DataAccess.Repository.product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessObject.product
{
    public class ProductService : BaseService<Product>, IProductService
    {

        public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
          
        }

        public IEnumerable<Product> GetProductsByCategoryId(int categoryId)
        {
          var products = _unitOfWork.BaseRepository<Product>().GetQuery(p => p.Cid == categoryId).ToList();
            return products;
          
        }
    }
}
