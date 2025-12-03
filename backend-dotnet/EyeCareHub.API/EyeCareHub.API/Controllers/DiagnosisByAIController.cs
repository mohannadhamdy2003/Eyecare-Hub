using AutoMapper;
using EyeCareHub.API.Dtos.DiagnosisHistory;
using EyeCareHub.API.Errors;
using EyeCareHub.API.Helper;
using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EyeCareHub.API.Controllers
{
    public class DiagnosisByAIController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManger;
        private readonly IAppointmentRepo _appointment;
        private readonly IDiagnosisRepo _diagnosisRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public DiagnosisByAIController(UserManager<AppUser> userManger, IAppointmentRepo appointment,
            IDiagnosisRepo diagnosisRepo, IMapper mapper, IConfiguration configuration)
        {
            _userManger = userManger;
            _appointment = appointment;
            _diagnosisRepo = diagnosisRepo;
            _mapper = mapper;
            _configuration = configuration;
        }
        [HttpPost("AddDiagnosis")]
        [Authorize(Roles ="Patient")]
        public async Task<ActionResult<DiagnosisAIResponse>> AddDiagnosis([FromForm]DiagnosisAIDto diagnosisAIDto)
        {
            ///Usermager
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return BadRequest(new ApiResponse(400,"Email Can Not Found Email."));

            var user = await _userManger.FindByEmailAsync(email);

            if (user == null)
             return BadRequest(new ApiResponse(401));
            

            var patient = await _appointment.GetPatientbyUserId(user.Id);

            if (patient == null)
                return BadRequest(new ApiResponse(401, "You are not  Login"));

            using var httpClient = new HttpClient();


            var fastApiUrl = $"{_configuration["URLDiagnosisByAIModel"]}";

            var jsonContent = JsonSerializer.Serialize(diagnosisAIDto);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // إرسال الطلب
            var response = await httpClient.PostAsync(fastApiUrl, content);

            if (!response.IsSuccessStatusCode)
                return BadRequest(new ApiResponse( 500,"Error In Ai Model"));
            
            var resultContent = await response.Content.ReadAsStringAsync();


            var diagnosisResult = JsonSerializer.Deserialize<DiagnosisAIResponse>(resultContent);


            var data = new DiagnosisHistory
            {
                ImageUploaded = await FileSetting.UploadFileAsync(diagnosisAIDto.ImageUploaded, "images", "DiagnosisHistory", HttpContext),
                DiagnosisResult = diagnosisResult.DiagnosisResult,
                ConfidenceScore = diagnosisResult.ConfidenceScore,
                PatientId = patient.Id
            };

            var resultAdd = await _diagnosisRepo.AddDiagnosisAsync(data);

            if (resultAdd == null)
                return BadRequest(new ApiResponse(500, "Error Server Can Not Add"));

            return Ok(diagnosisResult);
        }
        [HttpGet("Get-DiagnosisHos-ForPatient")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<IReadOnlyList<DiagnosisAIResponse>>> GetDiagnosis()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return BadRequest(new ApiResponse(400, "Email Can Not Found Email."));

            var user = await _userManger.FindByEmailAsync(email);

            if (user == null)
                return BadRequest(new ApiResponse(401));


            var patient = await _appointment.GetPatientbyUserId(user.Id);
            if (patient == null)
                return BadRequest(new ApiResponse(401));

            var diagnosisHos= await _diagnosisRepo.GetDiagnosisAsync(patient.Id);

            var data = _mapper.Map<DiagnosisAIResponse>(diagnosisHos);

            return Ok(data);



        }
    }
}
