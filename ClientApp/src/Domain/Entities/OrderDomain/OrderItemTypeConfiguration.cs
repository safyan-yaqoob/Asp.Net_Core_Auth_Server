using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities.OrderDomain
{
    public class OrderItemTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_item");

            builder.HasKey(c => c.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.Property(x => x.ProductName)
                .IsRequired();
        }
    }
}
