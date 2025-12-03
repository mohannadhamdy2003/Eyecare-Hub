using AutoMapper;
using EyeCareHub.API.Dtos;
using EyeCareHub.API.Dtos.AuthDtos;
using EyeCareHub.API.Errors;
using EyeCareHub.API.Extensions;
using EyeCareHub.API.Helper;
using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.Repositories;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.API.Controllers
{

    public class AcountController : BaseApiController
    {
        #region Inject
        private readonly UserManager<AppUser> userManger;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly IAdminRepo repo;
        private readonly IDoctorRepo docrepo;
        private readonly string URLFront;

        public AcountController(UserManager<AppUser> userManger, SignInManager<AppUser> signInManager,
                                  ITokenService tokenService, IMapper mapper,
                               IAdminRepo repo, IDoctorRepo docrepo, IConfiguration configuration)
        {
            this.userManger = userManger;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.repo = repo;
            this.docrepo = docrepo;
            URLFront = configuration["URLFrontEnd"];
        }
        #endregion


        #region register

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (CheckEmailExists(registerDto.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new[] { "This email is already in use" } });
            var user = new AppUser
            {
                UserName = registerDto.Email.Split("@")[0],
                PhoneNumber = registerDto.PhoneNumber,
                Email = registerDto.Email,
                DisplyName = registerDto.DisplayName,
                Address = new Address
                {
                    Country = registerDto.Country,
                    City = registerDto.City,
                    Street = registerDto.Street,
                },
                EmailConfirmed = false 
            };

            var result = await userManger.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse { Errors = result.Errors.Select(e => e.Description).ToArray() }); ;

            //////////////////////////////////////////
            var confirmationToken = await userManger.GenerateEmailConfirmationTokenAsync(user);

            var userId = user.Id;
            var encodedToken = System.Web.HttpUtility.UrlEncode(confirmationToken); // لو ASP.NET Core لازم تضيف using System.Web
            var confirmationLink = $"{URLFront}?userId={userId}&token={encodedToken}";

            await EmailSetting.SendEmailAsync(
                user.Email,
                "Confirm your email",
                $"Please confirm your account by clicking this link Token =[{confirmationToken}]: <a href='{confirmationLink}'>Confirm Email</a>"
            );

            return Ok(new { message = "Registration successful! Please check your email to confirm your account." });

        }

        [HttpPost("Send-ComfirmToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendComfirmToken(string Email)
        {
            var user = await userManger.FindByEmailAsync(Email);
            if (user == null)
                return NotFound(new ApiResponse(404));

            var confirmationToken = await userManger.GenerateEmailConfirmationTokenAsync(user);

            var userId = user.Id;
            var encodedToken = System.Web.HttpUtility.UrlEncode(confirmationToken); // لو ASP.NET Core لازم تضيف using System.Web
            var confirmationLink = $"{URLFront}?userId={userId}&token={encodedToken}";

            await EmailSetting.SendEmailAsync(
                user.Email,
                "Confirm your email",
                $"Please confirm your account by clicking this link Token =[{confirmationToken}]: <a href='{confirmationLink}'>Confirm Email</a>"
            );

            return Ok("Confirmation email sent.Please check your inbox. ");
        }

        [HttpPost("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> confirmation(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest(new ApiResponse(400, "Invalid confirmation request"));

            var user = await userManger.FindByIdAsync(userId);
            if (user == null) 
                return BadRequest(new ApiResponse(404, "User not found"));

            var result = await userManger.ConfirmEmailAsync(user, token);

            if(!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse { Errors = result.Errors.Select(e => e.Description).ToArray() }); ;
            return Ok("Email confirmed successfully! You can now log in.");

        }

        #endregion

        #region Login

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto model)
        {
            var user = await userManger.FindByEmailAsync(model.Email);

            if (user == null || !await userManger.CheckPasswordAsync(user, model.Password))
                return BadRequest(new ApiResponse(401));
            

            if (!await userManger.IsEmailConfirmedAsync(user))
                return BadRequest(new ApiResponse(401, "Please confirm your email before logging in."));

            var roles = await userManger.GetRolesAsync(user);

            return Ok(new UserDto()
            {
                Id =user.Id,
                Email = user.Email,
                DisplayName = user.DisplyName,
                Role = roles.FirstOrDefault(),
                Token = await tokenService.CreateToken(user, userManger)
            });
        }


        #endregion


        #region RestPassword
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ForgotPassword ([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var user = await userManger.FindByEmailAsync(forgotPasswordDto.Email);

            if (user == null)
                return BadRequest(new ApiResponse(404, "User Not Found"));

            var token = await userManger.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var Link = $"{encodedToken}";

            await EmailSetting.SendEmailAsync(
                 user.Email,
                "Reset Your Password",
                $"To reset your password, please click the following link{user.Id}: <a href='{Link}'>Reset Password</a>"
            );

            return Ok("Password reset link has been sent to your email.");
        }

        [HttpPost("Rest-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RestPassword(RestPasswordDto restPasswordDto)
        {
            var user = await userManger.FindByIdAsync(restPasswordDto.UserId);
            if (user == null)
                return BadRequest(new ApiResponse(401));

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(restPasswordDto.token));

            var result = await userManger.ResetPasswordAsync(user, decodedToken, restPasswordDto.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse { Errors = result.Errors.Select(e => e.Description).ToArray() });

            return Ok("Password has been reset successfully");
        }

        #endregion


        #region DeleteAcount

        //[HttpDelete("delete-account")]
        //[Authorize]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> DeleteAccount()
        //{

        //    var userDto = await GetCurrentUser();


        //    if (userDto == null)
        //        return Unauthorized(new ApiResponse(401));

        //    var userDtoValue = userDto.Value;
        //    var user = await userManger.FindByEmailAsync(userDtoValue.Email);

        //    var isAdmin = await userManger.IsInRoleAsync(user, "Admin");

        //    if (isAdmin)
        //        return BadRequest(new ApiResponse(400, "Admin accounts cannot be deleted."));
        //    // حذف الاكونت
        //    var result = await userManger.DeleteAsync(user);
        //    if (result.Succeeded)
        //        return Ok(new { message = "Account deleted successfully." });

        //    return BadRequest(new { message = "Error deleting account.", errors = result.Errors.Select(e => e.Description) });
        //}

        #endregion

        #region RegisteredAsPatient


        [HttpPost("register-as-patient")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> RegisterAsPatient([FromQuery] PatientRegistrationDto patientDto)
        {

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return Unauthorized(new ApiResponse(401));

            var user = await userManger.FindByEmailAsync(email);

            if (user == null)
                return Unauthorized(new ApiResponse(401));

            var userWithPatient = await userManger.FindUserWithPatientAndAddressByEmailAsync(user.Email);
            
            if (userWithPatient == null)
                return Unauthorized(new ApiResponse(401, "User not found."));

            if (user.patient != null)
                return Unauthorized(new ApiResponse(400, "You are a registered act"));

            var patient = mapper.Map<Patient>(patientDto);

            var result = await repo.AddPatient(patient,user);
            if (result == false)
                return BadRequest(new ApiResponse(401, "Can Not"));

            await userManger.AddToRoleAsync(user,"Patient");

            return Ok(new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplyName,
                Role = "Patient",
                Token = await tokenService.CreateToken(user, userManger)
            });

        }


        #endregion


        #region AddDoctor
        [HttpPost("Add-Doctor-byAdmin")]
        [Authorize(Roles="Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDoctor([FromForm]DoctorInfoDto doctorDto)
        {
            var appUser = mapper.Map<AppUser>(doctorDto);
            var IsAlready = await userManger.FindByEmailAsync(appUser.Email);
            if (IsAlready != null)
            {
                return BadRequest(new ApiResponse (400,"The Email Is Aready Used"));
            }
            var result = await userManger.CreateAsync(appUser, doctorDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors= result.Errors.Select(e => e.Description).ToList(), Message="Create User Failed"});
            }

            var doctor = mapper.Map<Doctor>(doctorDto);
            if (doctorDto.Picture == null)
            {
                doctor.PictureUrl = await FileSetting.UploadFileAsync(doctorDto.Picture, "images", "DoctorInfo", HttpContext);
            }
            doctor.AppUserId = appUser.Id;
            
            var resultAdd = await repo.AddDoctor(doctor, appUser);

            if (!resultAdd)
                return BadRequest(new ApiResponse(500, "Failed to save doctor details."));

            return Ok(new { message = "Doctor added successfully", doctorId = doctor.Id });


        }

        #endregion


        #region DeleteDoctor
        //[HttpDelete("Delete-Doctor-byAdmin")]
        //[Authorize(Roles = "Admin")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> DeleteDoctor([FromBody] int DoctorId)
        //{

        //    // إنشاء AppUser جديد
        //    var doctor = await docrepo.GetDoctorById(DoctorId);
            
        //    if (doctor != null)
        //    {
        //        return BadRequest(new ApiResponse(400, "The Email Is Aready Used"));
        //    }

            

        //    var result = await repo.DeleteDoctor(doctor);


        //    // حفظ التغييرات
        //    if (!result)
        //        return BadRequest(new ApiResponse(500, "Failed to Delete doctor details."));

        //    return Ok("Doctor Delete successfully");


        //}

        #endregion

        [Authorize]
        [HttpGet("Get-CurrentEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return BadRequest(new ApiResponse(400));

            var user = await userManger.FindByEmailAsync(email);

            if (user == null)
                return BadRequest(new ApiResponse(400));

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplyName,
                Email = email,
                Token = token   //////// TRY
            });
        }

        [HttpGet("emailexists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return Ok(await userManger.FindByEmailAsync(email) != null);
        }


    }
}
