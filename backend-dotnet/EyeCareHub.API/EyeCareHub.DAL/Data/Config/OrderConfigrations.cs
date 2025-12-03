using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeCareHub.DAL.Entities.OrderAggregate;

namespace EyeCareHub.DAL.Data.Config
{
    public class OrderConfigrations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShipToAddress, Address => Address.WithOwner()); 

            builder.OwnsOne(o => o.ShipToAddress, sa =>
            {
                sa.Property(a => a.Country).IsRequired();
                sa.Property(a => a.City).IsRequired();
                sa.Property(a => a.Street).IsRequired();
            });


            builder.Property(O => O.Status)
                .HasConversion(
                    OStatus => OStatus.ToString(), // دا الي رايح للداتا بيز هيبقا string
                    OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus) // دا الي هيرجع للفرونت اند
                    );

            builder.Property(O => O.Subtotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(O => O.Subtotal)
                .HasColumnType("decimal(18,2)");

            //builder.HasMany(O => O.Items).WithOne().OnDelete(DeleteBehavior.Cascade); //علي شان لما نمسح الاوردر كل الايتم تتمسح

            builder.HasMany(O => O.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
