using Merchandising.DTO.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Merchandising.VM.Portal
{
    public class UserVM
    {
        [Display(Name="Branch")]
        public IEnumerable<SelectListItem> BranchOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Role")]
        public IEnumerable<SelectListItem> AccessRoleOption { get; set; } = new List<SelectListItem>();
        [Display(Name = "Status")]
        public IEnumerable<SelectListItem> StatusOption { get; set; } = new List<SelectListItem>();
        public Users Users { get; set; }
    }

    public class Branches
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class AccessRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Status
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
