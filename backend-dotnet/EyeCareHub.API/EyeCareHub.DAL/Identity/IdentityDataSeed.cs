using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.HelperDLL;
using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.ComponentModel;
using EyeCareHub.DAL.Entities.Identity;

namespace EyeCareHub.DAL.Identity
{
    public class IdentityDataSeed
    {
        public static async Task SeedAsync(AppIdentityDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {

			try
			{
                if (!await roleManager.Roles.AnyAsync())
                {
                    var UserRoles = File.ReadAllText("../EyeCareHub.DAL/Identity/DataSeed/roles.json"); //EyeCareHub.DAL
                    var roles = JsonSerializer.Deserialize<List<IdentityRole>>(UserRoles);
                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }
                    await context.SaveChangesAsync();
                }

                if (!await userManager.Users.AnyAsync())
                {
                    var Admin = File.ReadAllText("../EyeCareHub.DAL/Identity/DataSeed/Admin.json"); //EyeCareHub.DAL
                    var Admins = JsonSerializer.Deserialize<List<AppUser>>(Admin);
                    foreach (var admin in Admins)
                    {
                        var result = await userManager.CreateAsync(admin, "M0stafa$"); // تعيين كلمة مرور افتراضية
                        if (result.Succeeded)
                        {
                            // Important: make sure role name matches exactly
                            await userManager.AddToRoleAsync(admin, "Admin");
                        }
                    }


                    var doctors = File.ReadAllText("../EyeCareHub.DAL/Identity/DataSeed/doctors.json"); //EyeCareHub.DAL
                                                                                                        //////////

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    options.Converters.Add(new HelperDLL.TimeSpanConverter());

                    var doctor = JsonSerializer.Deserialize<List<AppUser>>(doctors, options);

                    //var doctor = JsonSerializer.Deserialize<List<AppUser>>(doctors);
                    foreach (var doc in doctor)
                    {
                        var result = await userManager.CreateAsync(doc, "M0stafa$"); // تعيين كلمة مرور افتراضية
                        if (result.Succeeded)
                        {
                            // Important: make sure role name matches exactly
                            await userManager.AddToRoleAsync(doc, "Doctor");
                        }
                    }


                }
            }
			catch (Exception)
			{

				throw;
			}
        }
    }
}
