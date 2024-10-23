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
    }
}
