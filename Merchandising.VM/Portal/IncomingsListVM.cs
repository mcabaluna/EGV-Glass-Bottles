using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Portal
{
    public class IncomingsListVM
    {
        /// <summary>
        /// DocEntry
        /// </summary>
        public int DocEntry { get; set; }
        /// <summary>
        /// DocNum
        /// </summary>
        public int DocNum { get; set; }
        /// <summary>
        /// PaymentNo
        /// </summary>
        public string PaymentNo { get; set; }
        /// <summary>
        /// BranchCode
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Series
        /// </summary>
        public string Series { get; set; }
        /// <summary>
        /// CardCode
        /// </summary>
        public string CardCode { get; set; }
        /// <summary>
        /// CardName
        /// </summary>
        public string CardName { get; set; }
        /// <summary>
        /// InvoiceNo
        /// </summary>
        public string InvoiceNo { get; set; }
        /// <summary>
        /// InvoiceType
        /// </summary>
        public string InvoiceType { get; set; }
        /// <summary>
        /// DueDate
        /// </summary>
        public DateTime DueDate { get; set; }
        /// <summary>
        /// GrossTotal
        /// </summary>
        public decimal GrossTotal { get; set; }
        /// <summary>
        /// Collections
        /// </summary>
        public decimal Collections { get; set; }
        /// <summary>
        /// Balance
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// DocTotal
        /// </summary>
        public decimal DocTotal { get; set; }
        /// <summary>
        /// AmountPaid
        /// </summary>
        public decimal AmountPaid { get; set; }
        /// <summary>
        /// ModeOfPayment
        /// </summary>
        public string ModeOfPayment { get; set; }
        /// <summary>
        /// DatePaid
        /// </summary>
        public DateTime DatePaid { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// CreatedById
        /// </summary>
        public string CreatedById { get; set; }
        /// <summary>
        /// ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// ModifiedById
        /// </summary>
        public string ModifiedById { get; set; }
    }
}
