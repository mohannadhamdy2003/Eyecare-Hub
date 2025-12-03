using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace EyeCareHub.API.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, IdentityRole>(options => // لاضافات الجداول ويمكن نحط قيود علي الادخال
            {
                
                options.Password.RequireDigit = true;           
                options.Password.RequiredLength = 8;            
                options.Password.RequireNonAlphanumeric = true; 
                options.Password.RequireUppercase = true;       
                options.Password.RequireLowercase = true;       

                
                options.User.RequireUniqueEmail = true; 

                
                options.Lockout.MaxFailedAccessAttempts = 5;      // عدد محاولات تسجيل الدخول الفاشلة قبل الإغلاق
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10); // مدة قفل الحساب بعد المحاولات الفاشلة
                

            })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // scheme for create 
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    // scheme for Check
                })
             .AddJwtBearer(options =>
              {
                  options.RequireHttpsMetadata = false;
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                      ValidateIssuer = true,
                      ValidIssuer = configuration["JWT:ValidIssuer"],
                      ValidateAudience = true,
                      ValidAudience = configuration["JWT:ValidAudience"],
                      ValidateLifetime = true,
                      ClockSkew = TimeSpan.Zero // لمنع التأخير في التحقق من انتهاء الصلاحية
                  };
              });

            return services;
        }
    }
}
