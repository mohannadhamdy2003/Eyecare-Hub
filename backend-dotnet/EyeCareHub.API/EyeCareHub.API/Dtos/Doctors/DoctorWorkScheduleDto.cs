using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using EyeCareHub.API.Helper;

namespace EyeCareHub.API.Dtos.Doctors
{
    public class DoctorWorkScheduleDto
    {

        [Required(ErrorMessage ="Days is Required")]
        [ValidWorkDays]
        public List<string> WorkDays { get; set; }
        [Required(ErrorMessage = "StartTime is Required")]
        public TimeSpan StartTime { get; set; }
        [Required(ErrorMessage = "EndTime is Required")]
        public TimeSpan EndTime { get; set; }
    }
}

