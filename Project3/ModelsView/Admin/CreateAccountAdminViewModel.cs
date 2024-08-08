using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView.Admin
{
    public class CreateAccountAdminViewModel
    {
        [Required(ErrorMessage = "Please select role !!!")]
        public string? RoleId { get; set; }
        public string? Id { get; set; }
        [Required]
        public string? Fullname { get; set; }

        public string? Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password does not match")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
