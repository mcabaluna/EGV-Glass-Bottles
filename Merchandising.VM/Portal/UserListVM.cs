using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchandising.Enums;

namespace Merchandising.VM.Portal

{
    public class UserListVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string BranchName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public DateTime? LastAccess { get; set; }
    }
}
