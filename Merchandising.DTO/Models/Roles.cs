using Merchandising.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Merchandising.DTO.Models
{
    [Table("Roles")]
    public class Roles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Role Id")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Field can't be empty.")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
    }
}
