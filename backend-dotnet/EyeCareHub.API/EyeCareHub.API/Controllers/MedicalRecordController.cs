using AutoMapper;
using EyeCareHub.API.Dtos.MedicalRecord;
using EyeCareHub.API.Errors;
using EyeCareHub.BLL.DtoBLL;
using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.Repositories;
using EyeCareHub.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EyeCareHub.API.Controllers
{
    public class MedicalRecordController : BaseApiController
    {
        private readonly IMedicalRecordRepo _recordRepo;
        private readonly UserManager<AppUser> _userManger;
        private readonly IAppointmentRepo _appointment;
        private readonly IMapper _mapper;

        public MedicalRecordController(IMedicalRecordRepo recordRepo,UserManager<AppUser> userManger
            , IAppointmentRepo appointment, IMapper mapper)
        {
            _recordRepo = recordRepo;
            _userManger = userManger;
            _appointment = appointment;
            _mapper = mapper;
        }


        [HttpPost("Add-MedicalRecord")]
        [Authorize(Roles ="Doctor")]
        public async Task<ActionResult> AddMedicalRecord([FromQuery]MedicalRecordDto recordDto)
        {
            var user = await GetCuurentUser();
            if (user == null)
            {
                return BadRequest(new ApiResponse(401));
            }
            var doctor = await _appointment.GetDoctorbyUserId(user.Id);

            if (doctor == null)
                return BadRequest(new ApiResponse(401, "You are not  Login"));

            var appintment = await _appointment.GetExistingAppointment(recordDto.AppointmentId);

            if (appintment.AppointmentStatus != AppointmentStatus.Confirmed)
                return BadRequest(new ApiResponse(400, "You do not have the authority to add appointment, There must be an appointment from the user(Are you Comfirme Appointment?)."));

            var record = _mapper.Map<MedicalRecord>(recordDto);
            record.DoctorId = doctor.Id;
            var result = await _recordRepo.AddMedicalRecord(record);

            if(!result)
                return BadRequest(new ApiResponse(500));
            return Ok("Add MedicalRecord successfully");
        }


        [HttpPut("Update-MedicalRecord")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> UpdateMedicalRecord(int recordId, [FromQuery] MedicalRecordDto recordDto)
        {
            var user = await GetCuurentUser();
            if (user == null)
            {
                return BadRequest(new ApiResponse(401));
            }
            var doctor = await _appointment.GetDoctorbyUserId(user.Id);

            if (doctor == null)
                return BadRequest(new ApiResponse(401, "You are not  Login"));

            var appintment = await _appointment.GetExistingAppointment(recordDto.AppointmentId);

            if (appintment.AppointmentStatus != AppointmentStatus.Confirmed)
                return BadRequest(new ApiResponse(400, "You do not have the authority to add appointment, There must be an appointment from the user(Are you Comfirme Appointment?)."));

            var record = await _recordRepo.GetRecordById(recordId);

            if(record == null)
                return NotFound(new ApiResponse(404,"Record Not Found"));

            _mapper.Map(recordDto,record);

            var result = await _recordRepo.UpdateMedicalRecord(record);

            if (!result)
                return BadRequest(new ApiResponse(500));
            return Ok("Update MedicalRecord successfully");
        }


        // Get For Patient
        // Update Get Appointment For Doctor


        [HttpGet("Get-Patient-With-MedicalRecord")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<PatientWithMedicaRecordDto>> GetPatientMedicalRecord(int patientId, int appointmentId)
        {
            var user = await GetCuurentUser();

            if (user == null)
            {
                return BadRequest(new ApiResponse(401));
            }
            var doctor = await _appointment.GetDoctorbyUserId(user.Id);

            if (doctor == null)
                return BadRequest(new ApiResponse(401, "You are not  Login"));


            var appintment = await _appointment.GetExistingAppointment(appointmentId);
            if (appintment == null)
                return BadRequest(new ApiResponse(404, "NotFount appointment"));

            if (appintment.AppointmentStatus == AppointmentStatus.Cancelled)
                return BadRequest(new ApiResponse(400, "You Not Have a Appointment."));

            var patient =await _recordRepo.GetPatientWithRecord(patientId);

            if (patient == null)
                return BadRequest(new ApiResponse(404,"Can Not Found Patient"));

            
            return Ok(patient);
        }

        

        private async Task<AppUser> GetCuurentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return null;

            var user = await _userManger.FindByEmailAsync(email);

            if (user == null)
                return null;


            return (user);
        }

    }
}
