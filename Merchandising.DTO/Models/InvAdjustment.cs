using Merchandising.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("InvAdjustment")]
    public class InvAdjustment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Doc Entry")]
        public int DocEntry { get; set; }
        [Display(Name = "Document No.")]
        public int DocNum { get; set; }
        [Display(Name = "Branch")]
        public string BranchCode { get; set; }
        [Display(Name = "Type")]
        public AdjustmentType Type { get; set; }
        [Display(Name = "Reason")]
        public string Reason { get; set; }
        [Display(Name = "Reference")]
        public string Reference { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Doc Total")]
        public decimal DocTotal { get; set; }
        [Display(Name = "Status")]
        public StatusType Status { get; set; }
        [Display(Name = "Invoice Status")]
        public InvoiceStatus InvoiceStatus { get; set; }
        [Display(Name = "Series")]
        public string Series { get; set; }
        [Display(Name = "Inv Adjustment #")]
        public string InvAdjustmentNo { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
        public List<InvAdjustmentLines> Lines { get; set; }
        public string ObjectType { get; set; }
    }
}