using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView.Identity
{
    public class RegisterUserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string? Fullname { get; set; }

        public string? Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password does not match!!!")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? EmailConfirmationCode { get; set; }
    }
}
