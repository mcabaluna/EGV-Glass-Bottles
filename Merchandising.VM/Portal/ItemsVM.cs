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
    public class ItemsVM
    {
        [Display(Name = "Status")]
        public IEnumerable<SelectListItem> StatusOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Item Group")]
        public IEnumerable<SelectListItem> ItemGroupOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "UoM Group")]
        public IEnumerable<SelectListItem> UomGroupOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "WTax")]
        public IEnumerable<SelectListItem> WTaxOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Series")]
        public IEnumerable<SelectListItem> SeriesOption { get; set; } = new List<SelectListItem>();
        public Items Items { get; set; }
    }
}
