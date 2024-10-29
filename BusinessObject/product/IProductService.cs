﻿using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.product
{
    public interface IProductService : IBaseService<Product>
    {
        IEnumerable<Product> GetProductsByCategoryId(int categoryId);
    }
}
