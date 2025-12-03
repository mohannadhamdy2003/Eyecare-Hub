using System;

namespace EyeCareHub.API.Dtos.Appointment
{
    public class AppointmentResponseDoctorDto
    {

        public int Id { get; set; }

        public string WorkDays { get; set; }
        public TimeSpan TimeAppointment { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
    }
}
