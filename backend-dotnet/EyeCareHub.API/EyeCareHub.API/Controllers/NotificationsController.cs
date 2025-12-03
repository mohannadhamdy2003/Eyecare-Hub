using EyeCareHub.API.Dtos.Appointment;
using EyeCareHub.API.Errors;
using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EyeCareHub.API.Controllers
{
    public class NotificationsController : BaseApiController
    {
        private readonly INotificationRepository _notRepo;
        private readonly UserManager<AppUser> _userManager;

        public NotificationsController(INotificationRepository notRepo,UserManager<AppUser> userManager)
        {
            _notRepo = notRepo;
            _userManager = userManager;
        }

        [HttpGet("GetAll-Notification")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<Notification>>> GetNotification()
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return BadRequest(new ApiResponse(400, "User email not found."));

            var user = await _userManager.FindByEmailAsync(userEmail);
            var data = await _notRepo.GetNotification(user.Id);

            return Ok(data);
        }
    }
}
