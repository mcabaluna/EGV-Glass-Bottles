using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("Incomings")]
    public class Incomings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string PaymentNo { get; set; }
        public string BranchCode { get; set; }
        public string Series{ get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        //public string InvoiceNo { get; set; }
        //public string InvoiceType { get; set; }
        public DateTime DueDate { get; set; }
        //public decimal GrossTotal { get; set; }
        //public decimal Collections { get; set; }
        //public decimal Balance { get; set; }
        //public decimal DocTotal { get; set; }
        public decimal AmountPaid { get; set; }
        public string ModeOfPayment { get; set; }
        public DateTime DatePaid { get; set; }
        public string Remarks { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
        public List<IncomingLines> Lines { get; set; }
        public string ObjectType { get; set; }
    }
}
