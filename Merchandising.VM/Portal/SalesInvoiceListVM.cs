using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Portal
{
    public class SalesInvoiceListVM
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
        /// BranchCode
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// CardCode
        /// </summary>
        public string CardCode { get; set; }
        /// <summary>
        /// CardName
        /// </summary>
        public string CardName { get; set; }
        /// <summary>
        /// Reference
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// DeliveryDate
        /// </summary>
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// DueDate
        /// </summary>
        public DateTime DueDate { get; set; }
        /// <summary>
        /// DocTotal
        /// </summary>
        public decimal DocTotal { get; set; }
        /// <summary>
        /// Discount
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// DiscountAmount
        /// </summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// WTaxAmount
        /// </summary>
        public decimal WTaxAmount { get; set; }
        /// <summary>
        /// VatAmount
        /// </summary>
        public decimal VatAmount { get; set; }
        /// <summary>
        /// GrossTotal
        /// </summary>
        public decimal GrossTotal { get; set; }
        /// <summary>
        /// CreateadOn
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
        /// <summary>
        /// Series
        /// </summary>
        public string Series { get; set; }
        /// <summary>
        /// SInvoice
        /// </summary>
        public string SInvoice { get; set; }
        /// <summary>
        /// TermId
        /// </summary>
        public string TermId { get; set; }
        /// <summary>
        /// PricelistId
        /// </summary>
        public string PricelistId { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
    }
}
