using EyeCareHub.BLL.DtoBLL;
using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class MedicalRecordRepo : IMedicalRecordRepo
    {
        private readonly IUnitOfWork<AppIdentityDbContext> _unitOfWork;
        private readonly AppIdentityDbContext _context;

        public MedicalRecordRepo(IUnitOfWork<AppIdentityDbContext> unitOfWork, AppIdentityDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<bool> AddMedicalRecord(MedicalRecord medicalRecords)
        {
            _unitOfWork.Repository<MedicalRecord>().Add(medicalRecords);
            return (await _unitOfWork.Complete()) > 0;
        }

        public async Task<bool> UpdateMedicalRecord(MedicalRecord medicalRecords)
        {
            _unitOfWork.Repository<MedicalRecord>().Update(medicalRecords);
            return (await _unitOfWork.Complete()) > 0;
        }

        public async Task<MedicalRecord> GetRecordById(int recordId)
        {
            return await _unitOfWork.Repository<MedicalRecord>().GetByIdAsync(recordId);
        }

        public async Task<PatientWithMedicaRecordDto> GetPatientWithRecord(int patientId)
        {
            var data =  await _context.Set<Patient>().Where(al => al.Id == patientId)
                .Select(p => new PatientWithMedicaRecordDto
                {
                    Id = p.Id,
                    Name = p.User != null ? p.User.DisplyName : "",
                    PhoneNumber = p.User != null ? p.User.PhoneNumber : "",
                    Age = p.Age,
                    HasDiabetes = p.HasDiabetes,
                    OtherMedicalHistory = p.OtherMedicalHistory,
                    MedicalRecords = p.MedicalRecords.Select(m => new MedicalRecordResponseDto
                    {
                        Id = m.Id,
                        DoctorName = m.Doctor != null && m.Doctor.User != null ? m.Doctor.User.DisplyName : "",
                        DoctorId = m.Doctor != null ? m.Doctor.Id : 0,
                        Diagnosis = m.Diagnosis,
                        Notes = m.Notes,
                        Date = m.Date
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            return data;
        }

    }
}
