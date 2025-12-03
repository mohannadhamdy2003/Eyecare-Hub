using EyeCareHub.BLL.DtoBLL;
using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly IUnitOfWork<AppIdentityDbContext> _unitOfWork;
        private readonly AppIdentityDbContext _context;
        private readonly INotificationRepository _notRepo;

        public AppointmentRepo(IUnitOfWork<AppIdentityDbContext> unitOfWork,AppIdentityDbContext context,INotificationRepository notRepo)
        {
           _unitOfWork = unitOfWork;
            _context = context;
            _notRepo = notRepo;
        }

        #region Appointment

        public async Task<bool> AddAppointment(Appointment appointment)
        {
            await _unitOfWork.Repository<Appointment>().Add(appointment);
            return (await _unitOfWork.Complete()) > 0;
        }

        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            _unitOfWork.Repository<Appointment>().Update(appointment);

            return (await _unitOfWork.Complete()) > 0;
        }

        public async Task<bool> CancelledAppointment(Appointment appointment)
        {
            appointment.AppointmentStatus = AppointmentStatus.Cancelled;
             return (await _unitOfWork.Complete()) > 0;
        }

        public async Task<bool> ConfirmeAppointment(Appointment appointment)
        {
            appointment.AppointmentStatus = AppointmentStatus.Confirmed;
            return (await _unitOfWork.Complete()) > 0;
        }

        public async Task<bool> CompleteAppointment(Appointment appointment)
        {
            appointment.AppointmentStatus = AppointmentStatus.Completed;
            return (await _unitOfWork.Complete()) > 0;
        }


        public async Task<IReadOnlyList<Appointment>> GetExistingAppointment(int patientId, int doctorId)
        {
            var data = await _unitOfWork.Repository<Appointment>().FindAsync(al => (al.PatientId == patientId && al.DoctorId == doctorId));
            return data ;
        }
        public async Task<Appointment> GetExistingAppointment(int appointmentId)
        {
            var data = await _unitOfWork.Repository<Appointment>().FindAsync(al => al.Id == appointmentId);
            return data.FirstOrDefault();
        }

        public async Task<IReadOnlyList<AppointmentReturnToPatientDto>> GetAllAppointmentToUser(int patientId)
        {
            //return await _context.Set<Appointment>().Where(al => al.PatientId == patientId).Include(al => al.Doctor).ToListAsync();
            return await _context.Set<Appointment>()
                .Where(a => a.PatientId == patientId)
                .Select(a => new AppointmentReturnToPatientDto
                {
                    Id = a.Id,
                    WorkDays = a.WorkDays.ToString(),
                    AppointmentStatus = a.AppointmentStatus.ToString(),
                    TimeAppointment = a.TimeAppointment,
                    DoctorId = a.Doctor.Id,
                    DoctorName = a.Doctor.Name,
                    rating = a.Doctor.Rating
                })
                .ToListAsync();
        }

        
        public async Task<IReadOnlyList<AppointmentReturnToDoctorDto>> GetAllAppointmentToDoctor(int doctorId)
        {
            //return await _context.Set<Appointment>().Where(al => al.DoctorId == doctorId).Include(al => al.Patient.User).ToListAsync();
            return await _context.Set<Appointment>()
                .Where(a => a.DoctorId == doctorId)
                .Select(a => new AppointmentReturnToDoctorDto
                {
                    Id = a.Id,
                    WorkDays= a.WorkDays.ToString(),
                    AppointmentStatus = a.AppointmentStatus.ToString(),
                    TimeAppointment = a.TimeAppointment,
                    PatientId = a.Patient.Id,
                    PatientName = a.Patient.User.DisplyName
                })
                .ToListAsync();


        }
        #endregion

        public async Task<Patient> GetPatientbyUserId(string userId)
        {
            var data = await _unitOfWork.Repository<Patient>().FindAsync(al => al.AppUserId == userId);

            return data.FirstOrDefault();
        }

        public async Task<Doctor> GetDoctorbyUserId(string userId)
        {
            var data = await _unitOfWork.Repository<Doctor>().FindAsync(al => al.AppUserId == userId);

            return data.FirstOrDefault();
        }


        public async Task<string> GetAppUserId(int appointId)
        {
            var data = await _unitOfWork.Repository<Appointment>().
                GetQueryable()
                .Where(a => a.Id == appointId)
                .Select(a => a.Patient.AppUserId)
                .FirstOrDefaultAsync();
            return data.ToString();
        }
    }
}
