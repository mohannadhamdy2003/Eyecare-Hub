using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.specifications.Doctor_Specifications;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class DoctorRepo : IDoctorRepo
    {
        #region Inject
        private readonly IUnitOfWork<AppIdentityDbContext> _unitOfWork;
        public DoctorRepo(IUnitOfWork<AppIdentityDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Schedule
        public async Task<bool> AddWorkSchedule(DoctorWorkSchedule doctorWorkSchedule)
        {
            await _unitOfWork.Repository<DoctorWorkSchedule>().Add(doctorWorkSchedule);
            return (await _unitOfWork.Complete() > 0);
        }
        public async Task<bool> UpdateWorkSchedule(DoctorWorkSchedule doctorWorkSchedule)
        {
            _unitOfWork.Repository<DoctorWorkSchedule>().Update(doctorWorkSchedule);
            return (await _unitOfWork.Complete() > 0);
        }
        public async Task<bool> DeleteWorkSchedule(DoctorWorkSchedule doctorWorkSchedule)
        {
            _unitOfWork.Repository<DoctorWorkSchedule>().Delete(doctorWorkSchedule);
            return (await _unitOfWork.Complete() > 0);
        }
        public async Task<DoctorWorkSchedule> GetExistingSchedule(int doctorId)
        {
            var data = await _unitOfWork.Repository<DoctorWorkSchedule>().FindAsync(al => al.DoctorId == doctorId);
            return data.FirstOrDefault();
        }

        #endregion

        #region Doctor
        public async Task<IReadOnlyList<Doctor>> GetAllDoctorWithSpec(DoctorSpecParams specParams)
        {
            var spec = new DoctorWithScheduleSpec(specParams);

            return await _unitOfWork.Repository<Doctor>().GetAllWithspecAsync(spec);

        }

        public async Task<Doctor> GetDoctorByIdWithSpec(int Id)
        {
            var spec = new DoctorWithScheduleSpec(Id);

            return await _unitOfWork.Repository<Doctor>().GetByIdWithspecAsync(spec);

        }

        public async Task<int> GetDoctorId(string userId)
        {
            var data = await _unitOfWork.Repository<Doctor>().FindAsync(al => al.User.Id == userId);
            return data.FirstOrDefault().Id;
        }

        public async Task<int> GeyCountDoctorsWithSpec(DoctorSpecParams specParams)
        {
            var spec = new CountDoctorsSpec(specParams);

            return await _unitOfWork.Repository<Doctor>().GetCountAsync(spec);

        }

        public async Task<Doctor> GetDoctorById(int Id)
        {
            return await _unitOfWork.Repository<Doctor>().GetByIdAsync(Id);
        }
        #endregion

        #region Rating

        public async Task<bool> AddRating(DoctorRating rating, Doctor doctor)
        {
            //doctor.NumberOfRating++;
            if (doctor.NumberOfRating == 0)
            {
                doctor.NumberOfRating++;
                doctor.Rating = rating.Value;
            }
            else
            {
                var oldRating = doctor.Rating / doctor.NumberOfRating;
                doctor.NumberOfRating++;
                doctor.Rating = (oldRating + rating.Value) / doctor.NumberOfRating;
            }

            _unitOfWork.Repository<Doctor>().Update(doctor);
            _unitOfWork.Repository<DoctorRating>().Update(rating);

            var result = await _unitOfWork.Complete();
            return result > 0;
        }

        public async Task<bool> UpdateRating(Doctor doctor, DoctorRating newDoctorrating, decimal old)
        {

            var OldRating = (doctor.Rating * doctor.NumberOfRating) - old;

            doctor.Rating = (OldRating + newDoctorrating.Value) / doctor.NumberOfRating;

            _unitOfWork.Repository<Doctor>().Update(doctor);
            _unitOfWork.Repository<DoctorRating>().Update(newDoctorrating);

            var result = await _unitOfWork.Complete();
            return result > 0;


        }

        public async Task<bool> DeleteRating(Doctor doctor, DoctorRating rating)
        {
            var OldRating = (doctor.Rating * doctor.NumberOfRating) - rating.Value;
            doctor.NumberOfRating--;
            doctor.Rating = OldRating / doctor.NumberOfRating;

            _unitOfWork.Repository<Doctor>().Delete(doctor);
            _unitOfWork.Repository<DoctorRating>().Delete(rating);

            var result = await _unitOfWork.Complete();
            return result > 0;
        }
        public async Task<DoctorRating> Getalreadyrated(string userId,int doctorId)
        {
            var data = await _unitOfWork.Repository<DoctorRating>().FindAsync(al => al.UserId == userId && al.DoctorId == doctorId);

            return data.FirstOrDefault();
        }


        #endregion

    }
}
