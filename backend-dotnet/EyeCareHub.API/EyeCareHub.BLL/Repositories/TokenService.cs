using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> CreateToken(AppUser user, UserManager<AppUser> userManger)
        {
            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.GivenName , user.DisplyName),

            };

            var userRoles = await userManger.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role)); 
            }
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));  // key 
            //var authKey = new SymmetricSecurityKey(Convert.FromBase64String(configuration["JWT:Key"]));
            //var authKey = new SymmetricSecurityKey(Convert.FromBase64String(configuration["JWT:Key"]));


            var token = new JwtSecurityToken(

                issuer: configuration["JWT:ValidIssuer"],  // Fixed Info
                audience: configuration["JWT:ValidAudience"],// Fixed Info
                expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DurationInDays"])),// Fixed Info

                claims: authClaim, //private Info
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)  //Defined Algorithm
                                                                                                    //signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)



                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
