﻿using DataAccess.Models;
using DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.product
{
    public interface IProductRepository : IBaseRepository<Product>
    {
    }
}
