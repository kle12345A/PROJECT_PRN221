using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int? Cid { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't exceed 100 characters")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters")]
        public string? Description { get; set; }

        public string? Content { get; set; }

        [Url(ErrorMessage = "Image must be a valid URL")]
        public string? Image { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Old price must be a positive number")]
        public decimal? PriceOld { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "New price must be a positive number")]
        public decimal? PriceNew { get; set; }

        [Required(ErrorMessage = "Create date is required")]
        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50, ErrorMessage = "Admin name can't exceed 50 characters")]
        public string? AdminCreate { get; set; }

        [StringLength(50, ErrorMessage = "Admin name can't exceed 50 characters")]
        public string? AdminUpdate { get; set; }

        public virtual Category? CidNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
