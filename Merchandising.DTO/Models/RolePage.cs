using Merchandising.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("RolePage")]
    public  class RolePage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Role Page Id")]
        public string RolePageId { get; set; }
        [Required(ErrorMessage = "Field can't be empty.")]
        [Display(Name = "Role Id")]
        public string RoleId { get; set; }
        [Required(ErrorMessage = "Field can't be empty.")]
        [Display(Name = "Module")]
        public string Module { get; set; }
        public string Page { get; set; }
        public string Button { get; set; }
        public bool Visible { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
    }
}
