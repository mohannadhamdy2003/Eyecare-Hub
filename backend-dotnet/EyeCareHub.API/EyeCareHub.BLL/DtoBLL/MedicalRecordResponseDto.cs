using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.DtoBLL
{
    public class MedicalRecordResponseDto
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public int DoctorId { get; set; }

        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
    }
}
