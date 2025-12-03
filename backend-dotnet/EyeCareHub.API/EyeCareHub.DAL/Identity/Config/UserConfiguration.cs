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
//    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
//    {
//        public void Configure(EntityTypeBuilder<AppUser> builder)
//        {
//            builder
//             .HasOne(d => d.patient)
//             .WithOne(u => u.User)
//             .HasForeignKey<Patient>(d => d.AppUserId);

//            builder
//                .HasOne(d => d.Address)
//                .WithOne(u => u.User)
//                .HasForeignKey<Address>(d => d.AppUserId);

//        }
//    }
//}
