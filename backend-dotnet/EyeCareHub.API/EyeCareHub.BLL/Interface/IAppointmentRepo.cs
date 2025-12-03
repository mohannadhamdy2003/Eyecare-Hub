using EyeCareHub.BLL.DtoBLL;
using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IAppointmentRepo
    {
        Task<bool> AddAppointment(Appointment appointment);
        Task<bool> UpdateAppointment(Appointment appointment);
        Task<bool> CancelledAppointment(Appointment appointment);
        Task<bool> ConfirmeAppointment(Appointment appointment);
        Task<bool> CompleteAppointment(Appointment appointment);
        Task<IReadOnlyList<Appointment>> GetExistingAppointment(int patientId, int doctorId);
        Task<Appointment> GetExistingAppointment(int appointmentId);
        Task<IReadOnlyList<AppointmentReturnToPatientDto>> GetAllAppointmentToUser(int patientId);
        Task<IReadOnlyList<AppointmentReturnToDoctorDto>> GetAllAppointmentToDoctor(int doctorId);
        Task<Patient> GetPatientbyUserId(string userId);
        Task<Doctor> GetDoctorbyUserId(string userId);
        Task<string> GetAppUserId(int AppointId);
    }
}
