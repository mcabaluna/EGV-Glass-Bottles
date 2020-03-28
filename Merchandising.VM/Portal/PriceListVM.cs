using Merchandising.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Portal
{
    public class PriceListVM
    {
        /// <summary>
        /// PricelistId
        /// </summary>
        public string PricelistId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// BasePricelist
        /// </summary>
        public string BasePricelist { get; set; }
        /// <summary>
        /// Factor
        /// </summary>
        public decimal Factor { get; set; }
        /// <summary>
        /// Status
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
        /// <summary>
        /// Lines
        /// </summary>
        public List<PricelistLinesVM> Lines { get; set; }
        /// <summary>
        /// UoMs
        /// </summary>
        public List<PricelistUoMVM> UoMs { get; set; }
        /// <summary>
        /// WholesalePrice
        /// </summary>
        public decimal WholesalePrice { get; set; }
        /// <summary>
        /// RetailPrice
        /// </summary>
        public decimal RetailPrice { get; set; }
    }
}
