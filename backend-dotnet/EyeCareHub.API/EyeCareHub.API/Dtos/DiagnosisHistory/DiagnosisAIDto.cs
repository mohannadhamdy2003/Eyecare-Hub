using EyeCareHub.DAL.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace EyeCareHub.API.Dtos.DiagnosisHistory
{
    public class DiagnosisAIDto
    {
        [Required(ErrorMessage = "Image Is Required")]
        public IFormFile ImageUploaded { get; set; }
    }
}
