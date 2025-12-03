using EyeCareHub.BLL.DtoBLL;
using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IMedicalRecordRepo
    {
        Task<bool> AddMedicalRecord(MedicalRecord medicalRecords);
        Task<bool> UpdateMedicalRecord(MedicalRecord medicalRecords);
        Task<MedicalRecord> GetRecordById(int recordId);

        Task<PatientWithMedicaRecordDto> GetPatientWithRecord(int patientId);
    }
}
