
using System;

namespace Merchandising.VM.Results
{
    public class BPBalance_Results
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
        /// DocType
        /// </summary>
        public string DocType { get; set; }
        /// <summary>
        /// SINo
        /// </summary>
        public string SINo { get; set; }
        /// <summary>
        /// Document
        /// </summary>
        public string Document => DocType + " " + SINo;
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Total
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// GrossTotal
        /// </summary>
        public decimal GrossTotal { get; set; }
        /// <summary>
        /// TransactionDate
        /// </summary>
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// DueDate
        /// </summary>
        public DateTime DueDate { get; set; }
        /// <summary>
        /// Reference
        /// </summary>
        public string Reference { get; set; }
    }
}
