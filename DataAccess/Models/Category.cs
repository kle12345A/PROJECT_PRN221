using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(100)] 
        public string Title { get; set; } = null!;

        public string? Icon { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int? Orders { get; set; }

        [MaxLength(200)]
        public string? Note { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [Required]
        public string AdminCreate { get; set; } = null!;

        public string? AdminUpdate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
