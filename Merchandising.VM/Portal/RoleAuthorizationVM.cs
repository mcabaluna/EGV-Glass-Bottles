using Merchandising.DTO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Merchandising.VM.Portal
{
    public class RoleAuthorizationVM
    {
        [Display(Name = "Roles")]
        public IEnumerable<SelectListItem> RolesOption { get; set; } = new List<SelectListItem>();
        public List<RolePage> RolePage = new List<RolePage>();
        public List<RoleMenus> RoleMenus = new List<RoleMenus>();
    }
}
