using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("SequenceDocument")]
    public class SequenceDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ObjectCode { get; set; }
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }
        [Display(Name ="Document Type")]
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
    }
}