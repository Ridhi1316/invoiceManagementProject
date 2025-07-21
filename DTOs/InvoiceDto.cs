// DTOs/ (Data Transfer Objects)
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InvoiceManagement1.DTOs
{
    public class InvoiceDto
    {
        
        public int Id { get; set; } 

        [Required(ErrorMessage = "Customer name is required.")]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Draft|Sent|Paid|Overdue)$", ErrorMessage = "Status must be either Draft, Sent, Paid, or Overdue.")]
        public string Status { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}
