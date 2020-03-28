using System;

namespace Merchandising.VM.Portal
{
    public class ItemLedgerListVM
    {
        /// <summary>
        /// /LedgerId
        /// </summary>
        public long LedgerId { get; set; }
        /// <summary>
        /// ItemCode
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// Branch
        /// </summary>
        public string Branch { get; set; }
        /// <summary>
        /// Warehouse
        /// </summary>
        public string Warehouse { get; set; }
        /// <summary>
        /// TransDate
        /// </summary>
        public decimal TransDate { get; set; }
        /// <summary>
        /// ModuleName
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// DocumentId
        /// </summary>
        public string DocumentId { get; set; }
        /// <summary>
        /// BeginningBalance
        /// </summary>
        public decimal BeginningBalance { get; set; }
        /// <summary>
        /// BaseQty
        /// </summary>
        public decimal BaseQty { get; set; }
        /// <summary>
        /// Qty
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// InventoryAction
        /// </summary>
        public string InventoryAction { get; set; }
        /// <summary>
        /// EndingBalance
        /// </summary>
        public decimal EndingBalance { get; set; }
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
