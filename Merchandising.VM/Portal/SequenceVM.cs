using Merchandising.DTO.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Merchandising.VM.Portal
{
    public class SequenceVM
    {
        [Display(Name = "Document")]
        public IEnumerable<SelectListItem> DocumentOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Status")]
        public IEnumerable<SelectListItem> StatusOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Document Type")]
        public IEnumerable<SelectListItem> DocumentTypeOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Branch")]
        public IEnumerable<SelectListItem> BranchOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Default Series")]
        public IEnumerable<SelectListItem> SeriesOption { get; set; } = new List<SelectListItem>();
        public SequenceTable Sequence { get; set; }
        public SequenceDocument SeqDocument { get; set; }
    }
}
