using EyeCareHub.BLL.specifications;
using EyeCareHub.BLL.specifications.Doctor_Specifications;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EyeCareHub.API.Extensions
{
    public static class UserManagerExtentions
    {
        public static async Task<AppUser> FindUserWithPatientAndAddressByEmailAsync(this UserManager<AppUser> userManger, string UserEmailer)
        {
            var user = await userManger.Users
                .Include(u => u.Address)
                .Include(u => u.patient) 
                .SingleOrDefaultAsync(u => u.Email == UserEmailer);

            return user;
        }
    }
}
