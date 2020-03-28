using Merchandising.DTO.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Merchandising.VM.Portal
{
    public class InvAdjustmentVM
    {
        [Display(Name = "Type")]
        public IEnumerable<SelectListItem> TypeOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Item")]
        public IEnumerable<SelectListItem> ItemOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "UoM")]
        public IEnumerable<SelectListItem> UoMOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Series")]
        public IEnumerable<SelectListItem> SeriesOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "InvStatus")]
        public IEnumerable<SelectListItem> InvStatusOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Whse")]
        public IEnumerable<SelectListItem> WhseOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Branch")]
        public IEnumerable<SelectListItem> BranchOption { get; set; } = new List<SelectListItem>();
        public InvAdjustment InvAdjustment { get; set; }
    }
}
