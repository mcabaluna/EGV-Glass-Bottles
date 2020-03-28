using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.DTO.Models
{
    [Table("Provinces")]
    public class Provinces
    {
        [Key]
        public string ProvCode { get; set; }
        public string ProvName { get; set; }
    }
}
