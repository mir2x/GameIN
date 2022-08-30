using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameIN.Models
{
    public class UserModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,15}$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        
        [Display(Name = "Re-type Password")]
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string RetypePassword { get; set; }
    }
}