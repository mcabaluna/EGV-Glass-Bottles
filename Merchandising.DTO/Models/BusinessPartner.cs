using Merchandising.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Merchandising.DTO.Models
{
    [Table("BusinessPartner")]
    public class BusinessPartner
    {
        [Key]
        [Display(Name = "Card Code")]
        public string CardCode { get; set; }
        [Display(Name = "Card Name")]
        public string CardName { get; set; }
        [Display(Name = "Card Type")]
        public string BpType { get; set; }
        [Display(Name = "BP Group Code")]
        public string BpCode { get; set; }
        public string Tin { get; set; }
        [Display(Name = "Vat Code")]
        public string VatCode { get; set; }
        public bool WithWTax { get; set; }
        public string Address { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        [Display(Name = "Pricelist")]
        public string PricelistId { get; set; }
        [Display(Name = "Payment Term")]
        public string TermId { get; set; }
        public decimal Balance { get; set; }    
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
        public List<BpAddress> BpAddresses { get; set; }
        public List<BpWTax> BpWTax { get; set; }
        public string Series { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        public string Remarks { get; set; }
        public string ObjectType { get; set; }
    }
}