using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.DTO.Models
{
    [Table("Cities")]
    public class Cities
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }
        public string ProvCode { get; set; }
        public string ProvName { get; set; }
        public string CityName { get; set; }
        public int AreaCode { get; set; }
        public int ZipCode { get; set; }
        public bool Status { get; set; }
    }
}
