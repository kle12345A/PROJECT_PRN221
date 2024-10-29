using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AdminCreate { get; set; } = null!;
        public string? AdminUpdate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
