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
                 .Include(od => od.Product) 
                 .Where(od => od.OrderId == orderId)
                 .ToListAsync();
        }

        public async Task<decimal> GetTotalPriceByStatusAsync(string status)
        {
           
            var totalPrice = await _unitOfWork.BaseRepository<Order>().GetQuery()
                .Where(o => o.Status == status)
                .Include(o => o.OrderDetails) 
                .SelectMany(o => o.OrderDetails) 
                .SumAsync(od => od.Price);

            return totalPrice;
        }


        public async Task<int> GetTotalOrdersCountAsync()
        {
           return await _unitOfWork.BaseRepository<Order>().GetQuery().CountAsync();
        }

      

        public async Task<int> GetPendingOrdersCountAsync(string status)
        {
            return await _unitOfWork.BaseRepository<Order>()
                .GetQuery()
                .CountAsync(o => o.Status == status);
        }



        public async Task<List<Dictionary<string, object>>> GetTotalQuantityPerProductAsync()
        {
            var orderDetails = await _unitOfWork.BaseRepository<OrderDetail>()
                .GetQuery()
                .Include(od => od.Product) // Bao gồm thông tin từ bảng Product
                .ToListAsync();

            var totalQuantityPerProduct = orderDetails
                .GroupBy(od => od.ProductId)
                .Select(g =>
                {
                    var product = g.First().Product; // Lấy thông tin sản phẩm từ nhóm
                    var dictionary = new Dictionary<string, object>
                    {
                { "ProductId", g.Key },
                { "TotalQuantity", g.Sum(od => od.Quantity) },
                { "ProductName", product.Title },
                { "ProductImage", product.Image }
                    };
                    return dictionary;
                })
                .OrderByDescending(p => p["TotalQuantity"])
                .ToList();

            return totalQuantityPerProduct;
        }



    }
}
