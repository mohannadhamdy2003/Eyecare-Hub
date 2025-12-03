using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IAdminRepo
    {
        Task<bool> AddPatient(Patient patient, AppUser user);
        Task<bool> AddDoctor(Doctor doctor, AppUser appUser);
        Task<bool> DeleteDoctor(Doctor doctor);
    }
}
