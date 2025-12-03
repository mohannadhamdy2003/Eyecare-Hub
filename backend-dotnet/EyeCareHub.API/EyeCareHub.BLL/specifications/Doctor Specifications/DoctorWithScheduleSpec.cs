using EyeCareHub.BLL.specifications.ArticlesSpecifications;
using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.Doctor_Specifications
{
    public class DoctorWithScheduleSpec : BaseSpecification<Doctor>
    {
        public DoctorWithScheduleSpec(DoctorSpecParams doctorparams) : base(P =>
        (string.IsNullOrEmpty(doctorparams.Search) || P.User.DisplyName.ToLower().Contains(doctorparams.Search)) &&
        (!doctorparams.MinConsultationFee.HasValue || P.ConsultationFee >= doctorparams.MinConsultationFee.Value) &&
        (!doctorparams.MaxConsultationFee.HasValue || P.ConsultationFee <= doctorparams.MaxConsultationFee.Value) &&
        (!doctorparams.YearsOfExperience.HasValue || P.YearsOfExperience >= doctorparams.YearsOfExperience.Value))
        {
            AddIncludes(P =>P.DoctorWorkSchedule);
            ApplyPagination((doctorparams.PageSize * (doctorparams.PageIndex - 1)), doctorparams.PageSize);

            AddOrderBy(P => P.Rating);


            if (!string.IsNullOrEmpty(doctorparams.Sort))
            {
                switch (doctorparams.Sort)
                {
                    case "YearsOfExperienceBy":
                        AddOrderBy(P => P.YearsOfExperience);
                        break;
                    case "ConsultationFeeBy":
                        AddOrderBy(P => P.ConsultationFee);
                        break;

                    default:
                        AddOrderBy(P => P.Rating);
                        break;
                }
            }
              
        }
        public DoctorWithScheduleSpec(int id) : base(P => P.Id == id)
        {
            AddIncludes(P => P.DoctorWorkSchedule);
            AddIncludes(P => P.socialLinks);
        }
    }
}
