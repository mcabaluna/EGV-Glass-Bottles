using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace Merchandising.DTO.Models
{
    [Table("ItemUoM")]
    public class ItemUoM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UoMItemId { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        //public virtual Items VItemCode { get; set; }
        public string UoM { get; set; }
        public decimal Quantity { get; set; }
        public string BaseUoM { get; set; }
        public decimal BaseQty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public bool isSmallestUoM { get; set; }
    }
}