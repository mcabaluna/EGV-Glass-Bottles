using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    public class SequenceTableLines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Series { get; set; }
        public int ObjectCode { get; set; }
        public int Id { get; set; }
        [StringLength(8)]
        [Display(Name = "Name")]
        public string SeriesName { get; set; }
        [Display(Name = "First No.")]
        public int InitialNum { get; set; }
        [Display(Name = "Next No.")]
        public int NextNumber { get; set; }
        [Display(Name = "Last No.")]
        public int LastNum { get; set; }
        [StringLength(20)]
        [Display(Name = "Prefix")]
        public string BeginStr { get; set; }
        [StringLength(20)]
        [Display(Name = "Suffix")]
        public string LastStr { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }
        public bool Locked { get; set; }
        [Display(Name = "Period Ind.")]
        public bool Indicator { get; set; }
        [Display(Name = "No. of Digits")]
        public int NumSize { get; set; }
        [StringLength(2)]
        public string DocSubType { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
        public string BranchCode { get; set; }
    }
}
