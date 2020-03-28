using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.DTO.Models
{
    [Table("ItemLedger")]
    public class ItemLedger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LedgerId { get; set; }
        public string ItemCode { get; set; }
        public string Branch { get; set; }
        public string Warehouse { get; set; }
        public decimal TransDate { get; set; }
        public string ModuleName { get; set; }
        public string DocumentId { get; set; }
        public decimal BeginningBalance { get; set; }
        public decimal BaseQty { get; set; }
        public decimal Qty { get; set; }
        public string InventoryAction { get; set; }
        public decimal EndingBalance { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
    }
}
