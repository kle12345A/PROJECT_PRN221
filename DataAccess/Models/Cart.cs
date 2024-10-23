using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Cart
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public int? CustomerId { get; set; }

        public virtual User? Customer { get; set; }
    }
}
