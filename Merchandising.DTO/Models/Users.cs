using Merchandising.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchandising.DTO.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [Required(ErrorMessage = "Field can't be empty.")]
        [Display(Name = "User Code")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Field can't be empty.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Field can't be empty.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public int Role { get; set; }
        public string BranchCode { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNo { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public bool Status { get; set; }
        public DateTime? LastAccess { get; set; }
    }
}