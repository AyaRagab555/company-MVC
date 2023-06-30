﻿using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models.Account
{
    public class ResetPasswordModelView
    {
        [Required(ErrorMessage = "Password is Required")]
        [MinLength(5, ErrorMessage = "MinLength is 5")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

    }
}
