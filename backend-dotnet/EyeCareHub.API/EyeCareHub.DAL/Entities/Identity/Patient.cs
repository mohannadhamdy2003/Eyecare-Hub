using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    public class Patient:BaseEntity
    {
        public int Age { get; set; }

        public MedicalHistory HasDiabetes{ get; set; }
        public string OtherMedicalHistory { get; set; }

        public string AppUserId { get; set; }

        public AppUser User { get; set; }

        public List<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<DiagnosisHistory>? DiagnosisHistories { get; set; } = new List<DiagnosisHistory>(); 

    }
}
