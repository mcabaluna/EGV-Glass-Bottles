using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("Pricelist")]
    public class Pricelist
    {
        [Key]
        [Display(Name = "Pricelist Id")]
        public string PricelistId { get; set; }
        [Display(Name = "Pricelist Name")]
        public string Name { get; set; }
        [Display(Name = "Base Pricelist")]
        public string BasePricelist { get; set; }
        public decimal Factor { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
        public List<PricelistLines> Lines { get; set; }
        public List<PricelistUoM> UoMs { get; set; }
        public string ObjectType { get; set; }
        public string Series { get; set; }
    }
}