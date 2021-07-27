using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CStructorSite.Models
{
    public class RegisterUser
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserPassword { get; set; }
        [Compare("Password")]
        public string ConfirmUserPassword { get; set; }
    }
}
