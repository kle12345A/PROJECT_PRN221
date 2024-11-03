using DataAccess.Models;
using DataAccess.Repository.Base;
using DataAccess.Repository.category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.order
{
	public class OrdersRepository : BaseRepository<Order>, IOrdersRepository
	{
		public OrdersRepository(PROJECT_PRN212Context context) : base(context)
		{

		}
	}
}
