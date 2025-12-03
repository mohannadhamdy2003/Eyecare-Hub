using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    public class DoctorWorkSchedule:BaseEntity
    {

        public WorkDays WorkDays { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }


        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
