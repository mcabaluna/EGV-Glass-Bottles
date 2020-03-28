using Merchandising.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("RoleMenus")]
    public class RoleMenus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Menu Id")]
        public string RoleMenuId { get; set; }
        [Required(ErrorMessage = "Field can't be empty.")]
        [Display(Name = "Role Id")]
        public string RoleId { get; set; }
        [Required(ErrorMessage = "Field can't be empty.")]
        [Display(Name = "Menu Name")]
        public string MenuName { get; set; }
        [Required(ErrorMessage = "Field can't be empty.")]
        [Display(Name = "SubMenu Name")]
        public string SubMenuName { get; set; }
        public bool Visible { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
        //public string Icon { get; set; }
        //public string Url { get; set; }
        //public int ParentId { get; set; }
    }
}
