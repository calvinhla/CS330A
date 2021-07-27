using System;
using System.Collections.Generic;

#nullable disable

namespace CStructorSite.Models
{
    public partial class Class
    {
        public Class()
        {
            UserClasses = new HashSet<UserClass>();
        }

        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public decimal ClassPrice { get; set; }

        public virtual ICollection<UserClass> UserClasses { get; set; }
    }
}
