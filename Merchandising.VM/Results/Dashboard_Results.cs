using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Merchandising.VM.Results
{
    public class Dashboard_Results
    {
        /// <summary>
        /// TotalSalesPerMonth
        /// </summary>
        public decimal TotalSalesPerMonth { get; set; }
        public decimal MonthlyCollection { get; set; }
        /// <summary>
        /// TotalSalesForTheDay
        /// </summary>
        public decimal TotalSalesForTheDay { get; set; }
        /// <summary>
        /// Collectionfortoday
        /// </summary>
        public decimal Collectionfortoday { get; set; }
        /// <summary>
        /// InvoiceCount
        /// </summary>
        public int DailySInvoiceCount { get; set; }
        /// <summary>
        /// ItemPerWhse
        /// </summary>
        public List<ItemsPerWhse> ItemPerWhse { get; set; }
        /// <summary>
        /// SaleableItems
        /// </summary>
        public List<SaleableItems> SaleableItems { get; set; }
        /// <summary>
        /// WarehouseOption
        /// </summary>
        [Display(Name = "Warehouse")]
        public IEnumerable<SelectListItem> WarehouseOption { get; set; } = new List<SelectListItem>();
        /// <summary>
        /// BranchOption
        /// </summary>
        [Display(Name = "Branch")]
        public IEnumerable<SelectListItem> BranchOption { get; set; } = new List<SelectListItem>();
    }
    public class ItemsPerWhse
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
        /// Items
        /// </summary>
        public string Items => ItemCode + " , " + ItemName;
        /// <summary>
        /// WhseCode
        /// </summary>
        public string WhseCode { get; set; }
        /// <summary>
        /// WhseName
        /// </summary>
        public string WhseName { get; set; }
        /// <summary>
        /// Warehouse
        /// </summary>
        public string Warehouse => WhseCode + " - " + WhseName;
        /// <summary>
        /// OnHand
        /// </summary>
        public decimal OnHand { get; set; }
    }
    public class SaleableItems
    {
        /// <summary>
        /// WhseCode
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// WhseName
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// Warehouse
        /// </summary>
        public string Branch => BranchCode + " - " + BranchName;
        /// <summary>
        /// ItemCode
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// ItemName
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// Items
        /// </summary>
        public string Items => ItemCode + " , " + ItemName;
        /// <summary>
        /// SoldQty
        /// </summary>
        public decimal SoldQty { get; set; }
    }
}
