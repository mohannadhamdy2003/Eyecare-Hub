using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class DiagnosisRepo : IDiagnosisRepo
    {
        private readonly IUnitOfWork<AppIdentityDbContext> _unitOfWork;

        public DiagnosisRepo(IUnitOfWork<AppIdentityDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddDiagnosisAsync(DiagnosisHistory diagnosisHistory)
        {
            await _unitOfWork.Repository<DiagnosisHistory>().Add(diagnosisHistory);
            return (await _unitOfWork.Complete()) > 0;
        }

        public async Task<IReadOnlyList<DiagnosisHistory>> GetDiagnosisAsync(int PatientId)
        {
            return await _unitOfWork.Repository<DiagnosisHistory>().FindAsync(d => d.PatientId == PatientId);
        }
    }
}
