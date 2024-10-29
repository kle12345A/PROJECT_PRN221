using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Address { get; set; }
        public string? Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? Status { get; set; }
        public string? Email { get; set; }
    }
}
