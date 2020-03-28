using Merchandising.DTO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Merchandising.VM.Portal
{
    public class BPGroupVM
    {
        [Display(Name = "Status")]
        public IEnumerable<SelectListItem> StatusOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "BP Type")]
        public IEnumerable<SelectListItem> BPTypeOption { get; set; } = new List<SelectListItem>();
        public BpGroup BpGroup { get; set; }
    }
}
