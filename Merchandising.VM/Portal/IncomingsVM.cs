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
    public class IncomingsVM
    {
        [Display(Name = "Status")]
        public IEnumerable<SelectListItem> StatusOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Customer")]
        public IEnumerable<SelectListItem> CustomerOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Mode Of Payment")]
        public IEnumerable<SelectListItem> ModeOfPaymentOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Series")]
        public IEnumerable<SelectListItem> SeriesOption { get; set; } = new List<SelectListItem>();
        public Incomings Incomings { get; set; }
    }
}
