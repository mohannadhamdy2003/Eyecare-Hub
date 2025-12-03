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
    public class DiagnosisHistory :BaseEntity
    {
        public string ImageUploaded { get; set; }
        public string DiagnosisResult { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? ConfidenceScore { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int PatientId { get; set; } 

        public Patient Patient { get; set; }
    }
}
