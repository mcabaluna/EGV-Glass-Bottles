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
    public class PurchaseInvoiceVM
    {
        [Display(Name = "Status")]
        public IEnumerable<SelectListItem> StatusOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Customer")]
        public IEnumerable<SelectListItem> CustomerOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Item")]
        public IEnumerable<SelectListItem> ItemOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Mode Of Payment")]
        public IEnumerable<SelectListItem> ModeOfPaymentOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Payment Terms")]
        public IEnumerable<SelectListItem> PaymentTermsOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "UoM")]
        public IEnumerable<SelectListItem> UoMOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Series")]
        public IEnumerable<SelectListItem> SeriesOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Vat")]
        public IEnumerable<SelectListItem> VatOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Whse")]
        public IEnumerable<SelectListItem> WhseOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Branch")]
        public IEnumerable<SelectListItem> BranchOption { get; set; } = new List<SelectListItem>();
        public PurchaseInvoice PurchaseInvoice { get; set; }
    }
}
