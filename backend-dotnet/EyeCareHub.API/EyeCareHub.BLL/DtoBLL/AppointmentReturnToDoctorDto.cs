using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.DtoBLL
{
    public class AppointmentReturnToDoctorDto
    {
        public int Id { get; set; }

        public string WorkDays { get; set; }
        public string AppointmentStatus { get; set; }
        public TimeSpan TimeAppointment { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
    }
}
