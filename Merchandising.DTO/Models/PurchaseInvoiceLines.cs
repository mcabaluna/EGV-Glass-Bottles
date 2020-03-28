using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("PurchaseInvoiceLines")]
    public class PurchaseInvoiceLines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LineId { get; set; }

        public int LineNum { get; set; }
        public int DocEntry { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public string UoM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal WTax { get; set; }
        public string Whse { get; set; }
        public decimal Vat { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal GrossTotal { get; set; }
    }
}