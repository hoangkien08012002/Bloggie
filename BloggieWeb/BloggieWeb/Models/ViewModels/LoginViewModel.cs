﻿using System.ComponentModel.DataAnnotations;

namespace BloggieWeb.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password has to be at least 6 characters")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
