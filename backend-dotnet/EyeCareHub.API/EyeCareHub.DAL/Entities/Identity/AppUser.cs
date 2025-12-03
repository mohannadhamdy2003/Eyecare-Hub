using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplyName { get; set; }
        public Address Address { get; set; }
        public Doctor? Doctor { get; set; }
        public Patient? patient { get; set; }

    }
}
