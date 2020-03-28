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
    public class BusinessPartnerVM
    {
        [Display(Name = "BP Type")]
        public IEnumerable<SelectListItem> BPTypeOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "BP Group")]
        public IEnumerable<SelectListItem> BPGroupOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "VAT")]
        public IEnumerable<SelectListItem> VATOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "WTax")]
        public IEnumerable<SelectListItem> WTaxOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Pricelist")]
        public IEnumerable<SelectListItem> PriceListOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Payment Terms")]
        public IEnumerable<SelectListItem> PaymentTermsOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Series")]
        public IEnumerable<SelectListItem> SeriesOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Provinces")]
        public IEnumerable<SelectListItem> ProvincesOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Cities")]
        public IEnumerable<SelectListItem> CitiesOption { get; set; } = new List<SelectListItem>();
        public BusinessPartner BusinessPartner { get; set; }
        public BpAddress BpAddress { get; set; }
        public BpWTax BpWTax { get; set; }
        public List<WTax> WTax { get; set; }
    }
}
