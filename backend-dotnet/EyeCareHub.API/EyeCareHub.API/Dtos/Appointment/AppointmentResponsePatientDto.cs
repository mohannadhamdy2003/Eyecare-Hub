using System.ComponentModel.DataAnnotations;
using System;

namespace EyeCareHub.API.Dtos.Appointment
{
    public class AppointmentResponsePatientDto
    {
        public int Id { get; set; }

        public string WorkDays { get; set; }
        public TimeSpan TimeAppointment { get; set; }
        public string DoctorName { get; set; }
    }
}
