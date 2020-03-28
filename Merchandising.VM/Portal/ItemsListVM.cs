using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.VM.Portal
{
    public class ItemsListVM
    {
        /// <summary>
        /// ItemCode
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// ItemName
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// GroupCode
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// WTaxId
        /// </summary>
        public string WTaxName { get; set; }
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
        /// Series
        /// </summary>
        public string Series { get; set; }
        /// <summary>
        /// WholesaleQty
        /// </summary>
        public decimal WholesaleQty { get; set; }
        /// <summary>
        /// isSellItem
        /// </summary>
        public bool isSellItem { get; set; }
        /// <summary>
        /// isPurchaseItem
        /// </summary>
        public bool isPurchaseItem { get; set; }
        /// <summary>
        /// isInvItem
        /// </summary>
        public bool isInvItem { get; set; }
    }
}
