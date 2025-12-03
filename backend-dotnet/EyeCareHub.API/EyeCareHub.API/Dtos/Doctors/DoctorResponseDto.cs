using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;

namespace EyeCareHub.API.Dtos.Doctors
{
    public class DoctorResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string specialty { get; set; }
        public string bio { get; set; }
        public string PictureUr { get; set; }
        public string ClinicAddress { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
        public decimal Rating { get; set; }
        public int NumberOfRating { get; set; } 

        public decimal UserRating { get; set; }
        public IReadOnlyList<String> WorkDays { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
