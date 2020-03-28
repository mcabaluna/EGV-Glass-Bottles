using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Portal
{
    public class PricelistUoMVM
    {
        /// <summary>
        /// PUomId
        /// </summary>
        public int PUomId { get; set; }
        /// <summary>
        /// LineNum
        /// </summary>
        public int LineNum { get; set; }
        /// <summary>
        /// PricelistId
        /// </summary>
        public string PricelistId { get; set; }
        /// <summary>
        /// PLineId
        /// </summary>
        public int PLineId { get; set; }
        /// <summary>
        /// UoMCode
        /// </summary>
        public string UoMCode { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Percentage
        /// </summary>
        public decimal Percentage { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
    }
}
