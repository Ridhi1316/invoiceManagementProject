using System.ComponentModel.DataAnnotations;

namespace InvoiceManagement1.DTOs
{
    public class InvoiceItemDto
    {
        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }
}
