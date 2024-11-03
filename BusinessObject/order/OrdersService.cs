using BusinessObject.news;
using DataAccess.Models;
using DataAccess.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.order
{
    public class OrdersService : BaseService<Order>, IOrdersService
    {
        public OrdersService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<OrderDetail>> GetOrderDetailsAsync(int orderId)
        {
            return await _unitOfWork.BaseRepository<OrderDetail>().GetQuery()
                 .Include(od => od.Product) // Bao gồm thông tin sản phẩm
                 .Where(od => od.OrderId == orderId)
                 .ToListAsync();
        }
    }
    }
