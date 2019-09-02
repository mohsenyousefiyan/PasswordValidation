using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidation.Models
{
    public class UserChangePassword
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
