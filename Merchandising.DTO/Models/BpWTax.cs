using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.DTO.Models
{
    [Table("BpWTax")]
    public class BpWTax
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BpWTaxId { get; set; }
        public string CardCode { get; set; }
        //public virtual BusinessPartner VCardCode { get; set; }
        public string WTCode { get; set; }
    }
}
