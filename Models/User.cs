using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CStructorSite.Models
{
    public partial class User
    {
        public User()
        {
            UserClasses = new HashSet<UserClass>();
        }

        public int UserId { get; set; }
        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string UserEmail { get; set; }
        [Required]
        public string UserPassword { get; set; }        
        public bool UserIsAdmin { get; set; }

        [NotMapped]
        [Compare("UserPassword")]
        public string ConfirmUserPassword { get; set; }

        public virtual ICollection<UserClass> UserClasses { get; set; }
    }
}
