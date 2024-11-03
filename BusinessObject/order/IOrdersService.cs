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
    }
}
