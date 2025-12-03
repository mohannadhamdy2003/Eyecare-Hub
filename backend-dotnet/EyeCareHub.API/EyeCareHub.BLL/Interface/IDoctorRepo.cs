using EyeCareHub.BLL.Repositories;
using EyeCareHub.BLL.specifications.Doctor_Specifications;
using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IDoctorRepo
    {
        Task<bool> AddWorkSchedule(DoctorWorkSchedule doctorWorkSchedule);
        Task<bool> UpdateWorkSchedule(DoctorWorkSchedule doctorWorkSchedule);
        Task<bool> DeleteWorkSchedule(DoctorWorkSchedule doctorWorkSchedule);
        Task<DoctorWorkSchedule> GetExistingSchedule(int doctorId);


        Task<IReadOnlyList<Doctor>> GetAllDoctorWithSpec(DoctorSpecParams specParams);
        Task<int> GeyCountDoctorsWithSpec(DoctorSpecParams specParams);
        Task<Doctor> GetDoctorByIdWithSpec(int Id);

        Task<int> GetDoctorId(string userId);

        Task<Doctor> GetDoctorById(int Id);


        Task<DoctorRating> Getalreadyrated(string userId, int doctorId);
        Task<bool> UpdateRating(Doctor doctor, DoctorRating newDoctorrating, decimal old);
        Task<bool> AddRating(DoctorRating rating, Doctor doctor);
        Task<bool> DeleteRating(Doctor doctor, DoctorRating rating);

    }
}
