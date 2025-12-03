using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.Doctor_Specifications
{
    public class CountDoctorsSpec:BaseSpecification<Doctor>
    {
        public CountDoctorsSpec(DoctorSpecParams doctorparams) : base(P =>
        (string.IsNullOrEmpty(doctorparams.Search) || P.User.DisplyName.ToLower().Contains(doctorparams.Search)) &&
        (!doctorparams.MinConsultationFee.HasValue || P.ConsultationFee >= doctorparams.MinConsultationFee.Value) &&
        (!doctorparams.MaxConsultationFee.HasValue || P.ConsultationFee <= doctorparams.MaxConsultationFee.Value) &&
        (!doctorparams.YearsOfExperience.HasValue || P.YearsOfExperience >= doctorparams.YearsOfExperience.Value))
        {
            
        }
    }
}
