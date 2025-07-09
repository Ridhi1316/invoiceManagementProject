// Represent your Entity/Database structure. These match the database table structure.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InvoiceManagement1.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        public string CustomerName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Status must be either (Draft, Sent, Paid, Overdue).")]
        public string Status { get; set; } = string.Empty; // Draft, Sent, Paid, Overdue

        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}

