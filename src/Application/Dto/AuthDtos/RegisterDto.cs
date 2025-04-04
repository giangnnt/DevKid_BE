﻿using DevKid.src.Application.Constant;
using System.ComponentModel.DataAnnotations;

namespace DevKid.src.Application.Dto.AuthDtos
{
    public class RegisterDto
    {
        [Required]
        [StringLength(30)]
        [RegularExpression(RegexConst.EMAIL, ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(20, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
        [RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 special character and 1 number")]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(15, ErrorMessage = "Phone number must be 0-15 characters")]
        [RegularExpression(RegexConst.PHONE_NUMBER, ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = null!;

        public string? AvatarUrl { get; set; }
    }
}
