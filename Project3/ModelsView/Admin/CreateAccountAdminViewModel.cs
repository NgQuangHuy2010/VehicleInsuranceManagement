﻿using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView.Admin
{
    public class CreateAccountAdminViewModel
    {
        [Required(ErrorMessage = "Chọn vai trò")]
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
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không trùng khớp")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
