using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class AdminRepo : IAdminRepo
    {
        private readonly IUnitOfWork<AppIdentityDbContext> _unitOfWork;
        private readonly UserManager<AppUser> userManger;

        public AdminRepo(IUnitOfWork<AppIdentityDbContext> unitOfWork,UserManager<AppUser> userManger)
        {
            _unitOfWork = unitOfWork;
            this.userManger = userManger;
        }
        public async Task<bool> AddDoctor(Doctor doctor, AppUser appUser)
        {
            await userManger.AddToRoleAsync(appUser, "Doctor");
            appUser.EmailConfirmed = true;
            await _unitOfWork.Repository<Doctor>().Add(doctor);
            var result =await _unitOfWork.Complete();
            return result > 0;
        }

        public async Task<bool> AddPatient(Patient patient, AppUser user)
        {
            //user.patient = patient;
            patient.AppUserId = user.Id;

            await _unitOfWork.Repository<Patient>().Add(patient);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }

        public async Task<bool> DeleteDoctor(Doctor doctor)
        {
            _unitOfWork.Repository<Doctor>().Delete(doctor);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }
    }
}
