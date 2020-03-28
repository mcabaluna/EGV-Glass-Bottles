using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("InvAdjustmentLines")]
    public class InvAdjustmentLines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Line Id")]
        public int LineId { get; set; }
        [Display(Name = "Doc Entry")]
        public int DocEntry { get; set; }
        [Display(Name = "Line Num")]
        public int LineNum { get; set; }
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }
        [Display(Name = "UoM")]
        public string UoM { get; set; }
        [Display(Name = "Warehouse")]
        public string Whse { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Line Total")]
        public decimal LineTotal { get; set; }
    }
}