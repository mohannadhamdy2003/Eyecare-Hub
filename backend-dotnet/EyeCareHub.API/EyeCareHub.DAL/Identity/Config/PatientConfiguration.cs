//using EyeCareHub.DAL.Entities.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EyeCareHub.DAL.Identity.Config
//{
//    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
//    {
//        public void Configure(EntityTypeBuilder<Patient> builder)
//        {
//            builder
//                .HasMany(d => d.MedicalRecords)
//                .WithOne(u => u.Patient)
//                .HasForeignKey(a => a.PatientId)
//                .OnDelete(DeleteBehavior.NoAction);  // التأكد من استخدام NoAction هنا
//            builder
//                .HasMany(d => d.Appointments)
//                .WithOne(u => u.Patient)
//                .HasForeignKey(a => a.PatientId)
//                .OnDelete(DeleteBehavior.NoAction);
//            builder
//                .HasMany(d => d.DiagnosisHistories)
//                .WithOne(u => u.Patient)
//                .HasForeignKey(a => a.PatientId)
//                .OnDelete(DeleteBehavior.NoAction);

//            إزالة OnDelete(DeleteBehavior.Restrict) هنا في حال تم تحديدها في مكان آخر
//            builder
//                .HasOne(p => p.User)
//                .WithOne(u => u.patient)
//                .HasForeignKey<Patient>(p => p.AppUserId)
//                .OnDelete(DeleteBehavior.Restrict);
//        }
//    }
//}
