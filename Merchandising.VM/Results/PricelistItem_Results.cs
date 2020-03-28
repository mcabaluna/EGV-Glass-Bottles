using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Results
{
    public class PricelistItem_Results
    {
        /// <summary>
        /// PricelistId
        /// </summary>
        public string PricelistId { get; set; }
        /// <summary>
        /// ItemCode
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// ItemName
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// WholeSalePrice
        /// </summary>
        public decimal WholeSalePrice { get; set; }
        /// <summary>
        /// RetailPrice
        /// </summary>
        public decimal RetailPrice { get; set; }
        /// <summary>
        /// WholeSaleQty
        /// </summary>
        public decimal WholeSaleQty { get; set; }
    }
}
