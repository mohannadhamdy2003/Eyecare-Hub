using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.DtoBLL
{
    public class PatientWithMedicaRecordDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public MedicalHistory? HasDiabetes { get; set; }
        public string OtherMedicalHistory { get; set; }

        public int? MedicalRecordResponseDtoId { get; set; }
        public List<MedicalRecordResponseDto>? MedicalRecords { get; set; }
    }
}
