using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Merchandising.Enums;

namespace Merchandising.DTO.Models
{
    [Table("PricelistUoM")]
    public class PricelistUoM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PUomId { get; set; }
        public int LineNum { get; set; }
        public string PricelistId { get; set; }
        //public virtual Pricelist VPricelistId { get; set; }
        public string UoMCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Percentage { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public string ItemId { get; set; }
    }
}