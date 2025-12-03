using AutoMapper;
using EyeCareHub.API.Dtos.Appointment;
using EyeCareHub.API.Errors;
using EyeCareHub.BLL.DtoBLL;
using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EyeCareHub.API.Controllers
{
    
    public class AppointmentController : BaseApiController
    {

        #region Inject

        private readonly IAppointmentRepo _appointment;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManger;
        private readonly IDoctorRepo _doctor;
        private readonly INotificationRepository _notRero;

        public AppointmentController(IAppointmentRepo appointment,IMapper mapper
            ,UserManager<AppUser> userManager, IDoctorRepo doctor,INotificationRepository notRero)
        {
            _appointment = appointment;
            _mapper = mapper;
            _userManger = userManager;
            _doctor = doctor;
            _notRero = notRero;
        }
        #endregion

        #region Appointment
        [HttpPost("book-Appointment")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult> BookAppointment([FromQuery] AppointmentDto appointmentDto)
        {
            var user = await GetCuurentUser();
            if (user == null)
            {
                return BadRequest(new ApiResponse(401));
            }

            var patient = await _appointment.GetPatientbyUserId(user.Id);

            if (patient == null )
                return BadRequest(new ApiResponse(401, "You Must Login As A Patient"));

            var isAreadyBook = await _appointment.GetExistingAppointment(patient.Id, appointmentDto.DoctorId);
            if (isAreadyBook != null)
                foreach (var item in isAreadyBook)
                {
                    if (item.AppointmentStatus != AppointmentStatus.Cancelled && item.AppointmentStatus != AppointmentStatus.Completed)
                        return BadRequest(new ApiResponse(400, "You are Booked"));
                }


            var Shedule = await _doctor.GetExistingSchedule(appointmentDto.DoctorId);

            if (Enum.TryParse<WorkDays>(appointmentDto.WorkDays, out var selectedDay))
                if (!((Shedule.WorkDays & selectedDay) == selectedDay))
                    return BadRequest(new ApiResponse(400, "Day Is Not In Schedule"));

            if (appointmentDto.TimeAppointment <= Shedule.StartTime|| appointmentDto.TimeAppointment >= Shedule.EndTime)
                return BadRequest(new ApiResponse(400, "Time Must be Between Start Time And EndTime. "));


            var appointment = _mapper.Map<Appointment>(appointmentDto);
            appointment.PatientId = patient.Id;

            var result = await _appointment.AddAppointment(appointment);

            if (!result)
                return BadRequest(new ApiResponse(500));
            
            return Ok("Appointment Add successfully");
        }


        [HttpPut("Update-Appointment")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult> UpdateAppointment([FromQuery] int appointmentId,AppointmentDto appointmentDto)
        {
            var user = await GetCuurentUser();
            if (user == null)
            {
                return BadRequest(new ApiResponse(401));
            }
            var patient = await _appointment.GetPatientbyUserId(user.Id);

            if (patient == null )
                return BadRequest(new ApiResponse(401, "You Must Login As A Patient"));

            var isAreadyBook = await _appointment.GetExistingAppointment(appointmentId);

            if (isAreadyBook == null)
                return BadRequest(new ApiResponse(400, "You are not Booking"));
            if (isAreadyBook.PatientId != patient.Id)
                return BadRequest(new ApiResponse(400,"Not your appointment"));
            
            if (isAreadyBook.AppointmentStatus != AppointmentStatus.Pending)
                if (isAreadyBook.AppointmentStatus == AppointmentStatus.Cancelled)
                {
                    return BadRequest(new ApiResponse(400, "The appointment is cancelled , you can Add new appointment"));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Can not Update , The appointment is Not Pending"));
                }
                    
            

            var Shedule = await _doctor.GetExistingSchedule(appointmentDto.DoctorId);

            if (Enum.TryParse<WorkDays>(appointmentDto.WorkDays, out var selectedDay))
                if (!((Shedule.WorkDays & selectedDay) == selectedDay))
                    return BadRequest(new ApiResponse(400, "Day Is Not In Schedule"));

            if (selectedDay == WorkDays.None)
                return BadRequest(new ApiResponse(400,"Must be day in Days work doctor"));

            if (appointmentDto.TimeAppointment < Shedule.StartTime || appointmentDto.TimeAppointment > Shedule.EndTime)
                return BadRequest(new ApiResponse(400, "Time Must be Between Start Time And EndTime. "));

            _mapper.Map(appointmentDto,isAreadyBook);

            var result = await _appointment.UpdateAppointment(isAreadyBook);

            if (!result)
                return BadRequest(new ApiResponse(500));

            return Ok("Appointment Add successfully");
        }


        [HttpPut("Cancelle-Appointment")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> CancelleAppointment(int appointmentId)
        {

            var user = await GetCuurentUser();
            if (user == null)
            {
                return BadRequest(new ApiResponse(401));
            }
            var doctor = await _appointment.GetDoctorbyUserId(user.Id);

            if (doctor == null)
                return BadRequest(new ApiResponse(401, "You Must Login As A doctor"));


            var isAreadyBook = await _appointment.GetExistingAppointment( appointmentId);

            if (isAreadyBook == null)
                return NotFound(new ApiResponse(400, "Appointment NotFount"));

            if (isAreadyBook.DoctorId != doctor.Id)
                return BadRequest(new ApiResponse(400, "Not Your Appointment"));
            
            if (isAreadyBook.AppointmentStatus == AppointmentStatus.Cancelled)
                return BadRequest(new ApiResponse(400, "The appointment is already cancelled"));

            if (isAreadyBook.AppointmentStatus == AppointmentStatus.Completed)
                return BadRequest(new ApiResponse(400, "The appointment is already Complete"));

            if (isAreadyBook.AppointmentStatus == AppointmentStatus.Confirmed)
                return BadRequest(new ApiResponse(400, "The appointment is Confirme"));
            
            if (isAreadyBook.AppointmentStatus == AppointmentStatus.Cancelled)
                return BadRequest(new ApiResponse(400, "The appointment is cancelled"));

            var result = await _appointment.CancelledAppointment(isAreadyBook);

            if (!result)
                return BadRequest(new ApiResponse(500));
            ////////////////
            
            var appuserId = await _appointment.GetAppUserId(appointmentId);
            
            NotificItem item = new NotificItem($"the Appointment with Dr.{doctor.Name} is cancelled", appuserId);

            Notification notification = new Notification { Items = item };
            await _notRero.AddNotification(notification);

            return Ok("Appointment Cancelled successfully");
        }

        [HttpPut("Confirme-Appointment")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> ConfirmeAppointment(int appointmentId)
        {
            var user = await GetCuurentUser();
            if (user == null)
            {
                return BadRequest(new ApiResponse(401));
            }
            var doctor = await _appointment.GetDoctorbyUserId(user.Id);

            if (doctor == null)
                return BadRequest(new ApiResponse(401, "You Must Login As A doctor"));


            var isAreadyBook = await _appointment.GetExistingAppointment(appointmentId);

            if (isAreadyBook == null)
                return NotFound(new ApiResponse(400, "Appointment NotFount"));
            if (isAreadyBook.DoctorId != doctor.Id)
                return BadRequest(new ApiResponse(400, "Not Your Appointment"));

            if (isAreadyBook.AppointmentStatus == AppointmentStatus.Cancelled)
                return BadRequest(new ApiResponse(400, "The appointment is already cancelled"));

            if (isAreadyBook.AppointmentStatus == AppointmentStatus.Completed)
                return BadRequest(new ApiResponse(400, "The appointment is already Complete"));

            if (isAreadyBook.AppointmentStatus == AppointmentStatus.Confirmed)
                return BadRequest(new ApiResponse(400, "The appointment is Confirme"));


            var result = await _appointment.ConfirmeAppointment(isAreadyBook);

            if (!result)
                return BadRequest(new ApiResponse(500));
            //////////////////
            var appuserId = await _appointment.GetAppUserId(appointmentId);

            NotificItem item = new NotificItem($"the Appointment with Dr.{doctor.Name} is Confirmed", appuserId);
            Notification notification = new Notification();
            notification.Items = item;
            await _notRero.AddNotification(notification);

            return Ok("Appointment Cancelled successfully");
        }

        [HttpPut("Complete-Appointment")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> CompleteAppointment(int appointmentId)
        {
            var user = await GetCuurentUser();
            if (user == null)
            {
                return BadRequest(new ApiResponse(401));
            }

            var doctor = await _appointment.GetDoctorbyUserId(user.Id);

            if (doctor == null)
                return BadRequest(new ApiResponse(401, "You Must Login As A Patient"));

            
            var isAreadyBook = await _appointment.GetExistingAppointment(appointmentId);

            if (isAreadyBook == null)
                return NotFound(new ApiResponse(400, "Appointment NotFount"));

            if (doctor.Id != isAreadyBook.DoctorId)
                return BadRequest(new ApiResponse(401, "it's not your appointment"));

            if (isAreadyBook.AppointmentStatus == AppointmentStatus.Cancelled)
            return BadRequest(new ApiResponse(400, "The appointment is already cancelled"));

            if (isAreadyBook.AppointmentStatus == AppointmentStatus.Completed)
                return BadRequest(new ApiResponse(400, "The appointment is already Complete"));

            if (isAreadyBook.AppointmentStatus != AppointmentStatus.Confirmed)
                return BadRequest(new ApiResponse(400, "The appointment is Not Confirme"));


            var result = await _appointment.CompleteAppointment(isAreadyBook);

            if (!result)
                return BadRequest(new ApiResponse(500));

            return Ok("Appointment Cancelled successfully");
        }

        [HttpGet("Get-Appointment-ForPatient")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<IReadOnlyList<AppointmentReturnToPatientDto>>> GetAppointmentPatient()
        {
            var user = await GetCuurentUser();
            if (user == null)
            {
                return BadRequest(new ApiResponse(401));
            }

            var patient = await _appointment.GetPatientbyUserId(user.Id);

            if (patient == null || !User.IsInRole("Patient"))
                return BadRequest(new ApiResponse(401, "You Must Login As A Patient"));

            var appointments = await _appointment.GetAllAppointmentToUser(patient.Id);

            if (appointments == null)
                return NotFound(new ApiResponse(404, "Appointment NotFount"));

            return Ok(appointments);

        }

        [HttpGet("Get-Appointment-ForDoctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IReadOnlyList<AppointmentReturnToDoctorDto>>> GetAppointmentDoctor()
        {
            var user = await GetCuurentUser();
            if (user == null)
            {
                return BadRequest(new ApiResponse(401));
            }

            var doctor = await _appointment.GetDoctorbyUserId(user.Id);

            if (doctor == null )
                return BadRequest(new ApiResponse(401, "You Must Login As A doctor"));

            var appointments = await _appointment.GetAllAppointmentToDoctor(doctor.Id);

            if (appointments == null)
                return NotFound(new ApiResponse(404, "Appointment NotFount"));


            return Ok(appointments);

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
