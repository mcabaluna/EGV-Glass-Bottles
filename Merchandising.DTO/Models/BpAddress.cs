using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.DTO.Models
{
    [Table("BpAddress")]
   public class BpAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BpAddressId { get; set; }
        public string CardCode { get; set; }
        //public virtual BusinessPartner VCardCode { get; set; }
        public string Block { get; set; }
        public string Street { get; set; }
        public int CityId { get; set; }
        public string ProvId { get; set; }
        public bool Status { get; set; }
        
    }
}
