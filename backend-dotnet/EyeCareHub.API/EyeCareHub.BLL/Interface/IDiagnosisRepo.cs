using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IDiagnosisRepo
    {
        Task<bool> AddDiagnosisAsync(DiagnosisHistory diagnosisHistory);
        Task<IReadOnlyList<DiagnosisHistory>> GetDiagnosisAsync(int PatientId);
    }
}
