using Merchandising.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("Vat")]
    public class Vat
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public decimal Percentage { get; set; }
        public bool Status { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
    }
}