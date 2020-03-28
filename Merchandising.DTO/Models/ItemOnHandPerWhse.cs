using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("ItemOnHandPerWhse")]
    public class ItemOnHandPerWhse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ItemCode { get; set; }
        //public virtual Items VItemCode { get; set; }
        public string WhseId { get; set; }
        public decimal OnHand { get; set; }
        public decimal Commited { get; set; }
        public decimal Ordered { get; set; }
        public decimal ItemCost { get; set; }
    }
}