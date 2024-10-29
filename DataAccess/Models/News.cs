using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class News
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? AdminCreate { get; set; }
        public string? AdminUpdate { get; set; }
    }
}
