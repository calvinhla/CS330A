﻿using System;
using System.Collections.Generic;

#nullable disable

namespace CStructorSite.Models
{
    public partial class UserClass
    {
        public int ClassId { get; set; }
        public int UserId { get; set; }

        public virtual Class Class { get; set; }
        public virtual User User { get; set; }
    }
}
