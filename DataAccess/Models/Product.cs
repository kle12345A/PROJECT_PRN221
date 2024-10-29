using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int? Cid { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public decimal? PriceOld { get; set; }
        public decimal? PriceNew { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? AdminCreate { get; set; }
        public string? AdminUpdate { get; set; }
        public int? Quantity { get; set; }

        public virtual Category? CidNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
