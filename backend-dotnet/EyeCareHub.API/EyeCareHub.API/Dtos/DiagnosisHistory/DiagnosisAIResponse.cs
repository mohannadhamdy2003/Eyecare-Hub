using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EyeCareHub.DAL.Entities.Identity;
using System;

namespace EyeCareHub.API.Dtos.DiagnosisHistory
{
    public class DiagnosisAIResponse
    {
        public string DiagnosisResult { get; set; }
        public decimal ConfidenceScore { get; set; }


        public string ImageUploaded { get; set; }
        public DateTime CreatedAt { get; set; } 
        public int PatientId { get; set; }
    }
}
