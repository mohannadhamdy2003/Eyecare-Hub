using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    public class Appointment:BaseEntity
    {

        //[Required] // التاريخ لازم يكون موجود
        //public DateTime DateTime { get; set; }
        public WorkDays WorkDays { get; set; }
        public TimeSpan TimeAppointment { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Pending;

        public int DoctorId { get; set; }
        public  Doctor Doctor { get; set; }


        public int PatientId { get; set; }
        public  Patient Patient { get; set; }


    }
}
