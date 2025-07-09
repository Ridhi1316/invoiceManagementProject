using System.ComponentModel.DataAnnotations;

namespace InvoiceManagement1.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice must be greater than 0.")]
        public decimal UnitPrice { get; set; }

        public decimal Total => Quantity * UnitPrice;

        public int InvoiceId { get; set; }
    }
}
