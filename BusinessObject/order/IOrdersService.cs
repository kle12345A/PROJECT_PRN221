using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.order
{
	public interface IOrdersService : IBaseService<Order>
	{
        Task<List<OrderDetail>> GetOrderDetailsAsync(int orderId);
        Task<int> GetTotalOrdersCountAsync();
        Task<decimal> GetTotalPriceByStatusAsync(string status);
        Task<int> GetPendingOrdersCountAsync(string status);
        Task<List<Dictionary<string, object>>> GetTotalQuantityPerProductAsync();
    }
}
