using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Merchandising.Enums;

namespace Merchandising.DTO.Models
{
    [Table("Warehouse")]
    public class Warehouse
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string BranchCode { get; set; }
        public Branch Branch { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
    }
}