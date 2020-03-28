using System;

namespace Merchandising.VM.Portal
{
    public class InvAdjustmentListVM
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
        /// Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// Reference
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// InvocieStatus
        /// </summary>
        public string InvoiceStatus { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// DocTotal
        /// </summary>
        public decimal DocTotal { get; set; }
        /// <summary>
        /// Series
        /// </summary>
        public decimal Series { get; set; }
        /// <summary>
        /// InvAdjustment
        /// </summary>
        public string InvAdjustmentNo { get; set; }
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
    }
}
