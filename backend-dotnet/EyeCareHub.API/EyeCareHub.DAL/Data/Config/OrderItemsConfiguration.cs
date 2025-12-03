using EyeCareHub.DAL.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Data.Config
{
    public class OrderItemsConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(O => O.ItemOrder, PItem => PItem.WithOwner());

            builder.Property(OI => OI.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
