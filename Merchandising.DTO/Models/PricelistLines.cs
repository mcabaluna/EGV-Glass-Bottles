using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Merchandising.Enums;

namespace Merchandising.DTO.Models
{
    [Table("PricelistLines")]
    public class PricelistLines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PLineId { get; set; }
        public int LineNum { get; set; }
        public string PricelistId { get; set; }
        //public virtual Pricelist VPricelistId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string UoMCode { get; set; }
        public UoM UoM { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }

        [Display(Name = "Wholesale Price")]
        public decimal WholesalePrice { get; set; }
        [Display(Name = "Retail Price")]
        public decimal RetailPrice { get; set; }
    }
}