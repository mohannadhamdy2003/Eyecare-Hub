using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.Appointment
{
    public class AppointmentDto
    {
        [Required(ErrorMessage = "Day Appointment Is Required")]
        public string WorkDays { get; set; }

        [Required(ErrorMessage = "Time Appointment Is Required")]
        public TimeSpan TimeAppointment { get; set; }

        [Required(ErrorMessage = "the DoctorId Is Required")]
        public int DoctorId { get; set; }
    }
}
