//using EyeCareHub.DAL.Entities.Identity;
//using EyeCareHub.DAL.Entities.OrderAggregate;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;

//namespace EyeCareHub.DAL.Identity.Config
//{
//    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
//    {
//        public void Configure(EntityTypeBuilder<Doctor> builder)
//        {
//            builder
//                .HasOne(d => d.DoctorWorkSchedule)
//                .WithOne(s => s.Doctor)
//                .HasForeignKey<DoctorWorkSchedule>(s => s.DoctorId);

//            builder
//                .HasMany(d => d.doctorRating)
//                .WithOne(r => r.Doctor)
//                .HasForeignKey(r => r.DoctorId);
//            builder
//                .Property(d => d.Rating)
//                .HasPrecision(5, 2);
//            builder
//                .Property(d => d.ConsultationFee)
//                .HasPrecision(5, 2);



//            //builder
//            //    .HasOne(d => d.DoctorWorkSchedule)
//            //    .WithOne(s => s.Doctor)
//            //    .HasForeignKey<DoctorWorkSchedule>(s => s.DoctorId)
//            //    .OnDelete(DeleteBehavior.NoAction);

//            //builder
//            //    .HasMany(d => d.doctorRating)
//            //    .WithOne(r => r.Doctor)
//            //    .HasForeignKey(r => r.DoctorId)
//            //    .OnDelete(DeleteBehavior.NoAction);

//            //builder
//            //    .HasMany(d => d.Appointments)
//            //    .WithOne(a => a.Doctor)
//            //    .HasForeignKey(a => a.DoctorId)
//            //    .OnDelete(DeleteBehavior.NoAction);


//        }
//    }
//}
