using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    public class Doctor:BaseEntity
    {
        public string Name { get; set; }

        public string PictureUrl { get; set; }
        public string ClinicAddress { get; set; }

        public string specialty { get; set; }
        public string bio { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
        //public string Availability { get; set; }

        public string? CVFileURL { get; set; }
        public decimal Rating { get; set; } = 0;
        public int NumberOfRating { get; set; } = 0;
        public int DoctorRatingId { get; set; }
        public List<DoctorRating> doctorRating { get; set; } = new List<DoctorRating>();
        
        public List<SocialLink> socialLinks { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public List<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>(); 
        

        //public int DoctorWorkScheduleId { get; set; }
        public DoctorWorkSchedule DoctorWorkSchedule {get; set;}

        public string AppUserId { get; set; }
        public AppUser User { get; set; }
    }
}
