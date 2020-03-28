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
    public class PricelistsVM
    {
        [Display(Name = "Status")]
        public IEnumerable<SelectListItem> StatusOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Series")]
        public IEnumerable<SelectListItem> SeriesOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "UoM")]
        public IEnumerable<SelectListItem> UoMOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Base Price List")]
        public IEnumerable<SelectListItem> BasePricelistOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Items")]
        public IEnumerable<SelectListItem> ItemOption { get; set; } = new List<SelectListItem>();
        public string Series { get; set; }
        public string SequenceNumber { get; set; }
        public Pricelist Pricelist { get; set; }
    }
}
