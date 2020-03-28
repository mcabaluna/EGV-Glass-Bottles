using Merchandising.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("SalesInvoice")]
    public class SalesInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Doc Entry")]
        public int DocEntry { get; set; }
        [Display(Name = "Document No.")]
        public int DocNum { get; set; }
        [Display(Name = "Branch")]
        public string BranchCode { get; set; }
        [Display(Name = "Card Code")]
        public string CardCode { get; set; }
        [Display(Name = "Card Name")]
        public string CardName { get; set; }
        [Display(Name = "Reference No.")]
        public string Reference { get; set; }
        [Display(Name = "Status")]
        public InvoiceType Status { get; set; }
        [Display(Name = "Posting Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Delivery Date")]
        public DateTime Deliverydate { get; set; }
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        [Display(Name = "Total")]
        public decimal DocTotal { get; set; }
        [Display(Name = "Discount")]
        public decimal Discount { get; set; }
        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }
        [Display(Name = "WTax")]
        public decimal WTaxAmount { get; set; }
        [Display(Name = "VAT")]
        public decimal VatAmount { get; set; }
        [Display(Name = "Gross Total")]
        public decimal GrossTotal { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
        public List<SalesInvoiceLines> Lines { get; set; }
        [Display(Name = "Series")]
        public string Series { get; set; }
        public string ObjectType { get; set; }
        [Display(Name = "Sales Invoice #")]
        public string SInvoice { get; set; }
        [Display(Name = "Payment Terms")]
        public string TermId { get; set; }
        [Display(Name = "Paid To Date")]
        public decimal PaidToDate { get; set; }
        public string PricelistId { get; set; }
        public string Remarks { get; set; }

    }
}