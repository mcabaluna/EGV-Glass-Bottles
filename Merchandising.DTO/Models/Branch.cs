using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Merchandising.Enums;

namespace Merchandising.DTO.Models
{
    [Table("Branch")]
    public class Branch
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
    }
}