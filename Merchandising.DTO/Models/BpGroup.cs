using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchandising.DTO.Models
{
    [Table("BpGroup")]
    public class BpGroup
    {
        [Key]
        public string Code { get; set; }
        public int? BpType { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
    }
}
