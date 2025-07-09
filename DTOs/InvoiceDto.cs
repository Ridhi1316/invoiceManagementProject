// DTOs/ (Data Transfer Objects)
// Prevent overposting and keep your API contracts clean.
// You don’t want to expose internal DB structure in your API.
// Controls API payload, protects DB integrity
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InvoiceManagement1.DTOs
{
    public class InvoiceDto
    {
        [Required]
        public int Id { get; set; } 

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}
