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
    public class MedicalRecord:BaseEntity
    {

        public string DoctorName { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public int DoctorId { get; set; }
        public  Doctor Doctor { get; set; }

        [StringLength(1000)]
        public string Diagnosis { get; set; }
        [StringLength(2000)]
        public string Notes { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
