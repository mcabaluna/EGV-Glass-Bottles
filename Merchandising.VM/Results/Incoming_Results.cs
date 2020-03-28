using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Results
{
    public class Incoming_Results
    {
        /// <summary>
        /// DocEntry
        /// </summary>
        public int DocEntry { get; set; }
        /// <summary>
        /// InvoiceNo
        /// </summary>
        public string InvoiceNo { get; set; }
        /// <summary>
        /// DocNum
        /// </summary>
        public int DocNum { get; set; }
        /// <summary>
        /// DueDate
        /// </summary>
        public DateTime DueDate { get; set; }
        /// <summary>
        /// CardCode
        /// </summary>
        public string CardCode { get; set; }
        /// <summary>
        /// CardName
        /// </summary>
        public string CardName { get; set; }
        /// <summary>
        /// GrossTotal
        /// </summary>
        public decimal GrossTotal { get; set; }
        /// <summary>
        /// Collection
        /// </summary>
        public decimal Collection { get; set; }
        /// <summary>
        /// Balance
        /// </summary>
        public decimal Balance { get; set; }

    }
}
