using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Merchandising.DTO.Models
{
    [Table("SalesInvoiceLines")]
    public class SalesInvoiceLines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LineId { get; set; }

        public int LineNum { get; set; }
        public int DocEntry { get; set; }
        //public virtual SalesInvoice VDocEntry { get; set; }
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