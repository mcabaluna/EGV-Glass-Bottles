using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("PaymentTerms")]
    public class PaymentTerms
    {
        [Key]
        [Display(Name = "Term Id")]
        public string TermId { get; set; }
        public string Name { get; set; }
        [Display(Name = "No. Of Days")]
        public int NoOfDays { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
    }
}