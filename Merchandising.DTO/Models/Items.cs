using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("Items")]
    public class Items
    {
        [Key]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        [Display(Name = "Group Code")]
        public string GroupCode { get; set; }
        [Display(Name = "WTax Id")]
        public string WtaxId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
        public List<ItemUoM> ItemUoM { get; set; }
        public List<ItemOnHandPerWhse> ItemOnHandPerWhse { get; set; }
        public string Series { get; set; }
        [Display(Name = "Wholesale Qty")]
        public decimal WholeSaleQty { get; set; }
        public string ObjectType { get; set; }
        public bool isSellItem { get; set; }
        public bool isPurchaseItem { get; set; }
        public bool isInvItem { get; set; }
    }
}