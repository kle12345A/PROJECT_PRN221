using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Banner
    {
        public int Id { get; set; }
        public string Image { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AdminCreate { get; set; } = null!;
        public string? AdminUpdate { get; set; }
    }
}
