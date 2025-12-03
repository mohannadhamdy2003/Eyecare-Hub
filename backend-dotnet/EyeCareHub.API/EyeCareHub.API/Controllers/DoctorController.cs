using EyeCareHub.API.Errors;
using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.Repositories;
using EyeCareHub.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using EyeCareHub.BLL.specifications.Doctor_Specifications;
using EyeCareHub.API.Dtos.Doctors;
using EyeCareHub.API.Helper;
using EyeCareHub.API.Dtos.ContentEducations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using EyeCareHub.API.Extensions;
using EyeCareHub.API.Dtos.AuthDtos;
using Microsoft.EntityFrameworkCore;

namespace EyeCareHub.API.Controllers
{
    public class DoctorController : BaseApiController
    {
        #region Inject
        private readonly IDoctorRepo _doctor;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManger;

        public DoctorController(IDoctorRepo doctor, IMapper mapper, UserManager<AppUser> userManger)
        {
            _doctor = doctor;
            _mapper = mapper;
            _userManger = userManger;
        }
        #endregion

        #region Doctor
        

        [HttpGet("GetAll-Doctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pagination<DoctorResponseDto>>> GetDoctors([FromQuery]DoctorSpecParams specParams)
        {
            var doctors = await _doctor.GetAllDoctorWithSpec(specParams);

            if (!doctors.Any())
                return NotFound(new ApiResponse(404, "Not Fonut Any Doctor"));

            var data = _mapper.Map<IReadOnlyList<DoctorResponseDto>>(doctors);
            var user =await GetCuurentUser();
            if (user != null) 
            {

                foreach (var doctor in data)
                {
                    var userRating = await _doctor.Getalreadyrated(user.Id, doctor.Id);
                    if (userRating != null) 
                    {
                        doctor.UserRating = userRating.Value;
                    }
                }
                
            }
            var count = await _doctor.GeyCountDoctorsWithSpec(specParams);
            return Ok(new Pagination<DoctorResponseDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }


        [HttpGet("Get-Doctor-ById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DoctorResponseDto>> GetDoctorById( int doctorId)
        {
            var doctor = await _doctor.GetDoctorByIdWithSpec(doctorId);

            if (doctor == null)
                return NotFound(new ApiResponse(404, "Not Fonut Any Doctor"));

            var data = _mapper.Map<DoctorResponseDto>(doctor);
            var user = await GetCuurentUser();
            if (user != null)
            {

                var userRating = await _doctor.Getalreadyrated(user.Id, doctor.Id);
                if (userRating != null)
                {
                    data.UserRating = userRating.Value;
                }
                

            }
            return Ok(data);
        }


        #endregion


        #region Rating

        [HttpPost("Add-Update-Rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult> AddUpdateRatting(doctorRatingDto ratingDto)
        {
            var doctor = await _doctor.GetDoctorById(ratingDto.doctorId);

            if (doctor == null)
                return NotFound(new ApiResponse(404, "Not Fonut Any Doctor"));
            
            var user = await GetCuurentUser();
            if (user == null)
                return BadRequest(new ApiResponse(401));
            //await _userManger.FindByIdAsync(user.Id);

            

            if (User.IsInRole("Doctor"))
            {
                return BadRequest(new ApiResponse(400,"Doctor Can Not Add Rating"));
            }

            var alreadyrated = await _doctor.Getalreadyrated(user.Id , ratingDto.doctorId);

            if (alreadyrated == null)
            {
                DoctorRating doctorRat = new DoctorRating
                {
                    UserId = user.Id,
                    DoctorId = ratingDto.doctorId,
                    Value = ratingDto.ratingValue,
                    Comments = ratingDto.Comments,
                };
                var resultAdd = await _doctor.AddRating(doctorRat, doctor);
                if (!resultAdd)
                    return BadRequest(new ApiResponse(500, "Failed to Update the Rating."));

                return Ok("Rating Add successfully");
            }
            var newRating = alreadyrated;
            newRating.Value = ratingDto.ratingValue;
            newRating.Comments = ratingDto.Comments;

            var result = await _doctor.UpdateRating(doctor, newRating, alreadyrated.Value);
            if (!result)
                return BadRequest(new ApiResponse(500, "Failed to Update the Rating."));




            return Ok("Rating Update successfully");
        }


        [HttpDelete("Delete-Rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult> DeleteRatting(int doctorId)
        {
            var doctor = await _doctor.GetDoctorById(doctorId);

            if (doctor == null)
                return NotFound(new ApiResponse(404, "Not Fonut Any Doctor"));

            var user = await GetCuurentUser();


            var alreadyrated = await _doctor.Getalreadyrated(user.Id, doctorId);

            if (alreadyrated == null)
                return BadRequest(new ApiResponse(400, "You Not Rating"));


            var result = await _doctor.DeleteRating(doctor, alreadyrated);
            if (!result)
                return BadRequest(new ApiResponse(500, "Failed to Delete the Rating."));

            return Ok("Rating Delete successfully");
        }

        #endregion



        #region WorkSchedule

        [HttpPost("add-WorkSchedule")]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddWorkSchedule(DoctorWorkScheduleDto scheduleDto)
        {
            if (scheduleDto.StartTime > scheduleDto.EndTime)
                return BadRequest(new ApiResponse(400, "can not Start Time Before End Time"));

            var user = await GetCuurentUser();
            var doctorId = await _doctor.GetDoctorId(user.Id);

            var schedule = _mapper.Map<DoctorWorkSchedule>(scheduleDto);

            schedule.DoctorId = doctorId;

            var result = await _doctor.AddWorkSchedule(schedule);
            if (!result)
                return BadRequest(new ApiResponse(500, "Failed to add the work schedule."));

            return Ok(new { message = "Work schedule added successfully", scheduleId = schedule.Id });
        }

        [HttpPut("updat-WorkSchedule")]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateWorkSchedule(DoctorWorkScheduleDto scheduleDto)
        {
            if (scheduleDto.StartTime > scheduleDto.EndTime)
                return BadRequest(new ApiResponse(400, "can not Start Time Before End Time"));

            var user = await GetCuurentUser();
            var doctorId = await _doctor.GetDoctorId(user.Id);
            var existingSchedule = await _doctor.GetExistingSchedule(doctorId);

            if (existingSchedule == null)
                return NotFound(new ApiResponse(404, "Work schedule not found."));

            _mapper.Map(scheduleDto, existingSchedule);


            var result = await _doctor.UpdateWorkSchedule(existingSchedule);

            if (!result)
                return BadRequest(new ApiResponse(500, "Failed to update the work schedule."));

            return Ok(new { message = "Work schedule updated successfully", scheduleId = existingSchedule.Id });
        }
        

        [HttpDelete("delete-WorkSchedule")]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteWorkSchedule()
        {
            var user = await GetCuurentUser();
            var doctorId =await _doctor.GetDoctorId(user.Id);

            var existingSchedule = await _doctor.GetExistingSchedule(doctorId);

            if (existingSchedule == null)
                return NotFound(new ApiResponse(404, "Work schedule not found."));

            var result = await _doctor.DeleteWorkSchedule(existingSchedule);

            if (!result)
                return BadRequest(new ApiResponse(500, "Failed to delete the work schedule."));

            return Ok("Work schedule deleted successfully");
        }


        [HttpGet("Get-Schedule")]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetWorkSchedulesForDoctor()
        {

            var user = await GetCuurentUser();
           
            var doctorId = await _doctor.GetDoctorId(user.Id);

            var existingSchedule = await _doctor.GetExistingSchedule(doctorId);



            if (existingSchedule == null)
                return NotFound(new ApiResponse(404, "No work schedules found ."));

            var scheduleDtos = _mapper.Map<DoctorWorkScheduleDto>(existingSchedule);

            return Ok(scheduleDtos);
        }


        #endregion

        private async Task<AppUser> GetCuurentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return null;

            var user = await _userManger.FindByEmailAsync(email);
            //var user = await _userManger.Users
            //    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.Trim().ToLower());

            if (user == null)
                return null;


            return (user);
        } 


    }
}
