using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.DtoBLL
{
    public class AppointmentReturnToPatientDto
    {
        public int Id { get; set; }

        public string WorkDays { get; set; }
        public string AppointmentStatus { get; set; }

        public TimeSpan TimeAppointment { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public decimal rating { get; set; }
    }
}
