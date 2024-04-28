using Domain;
using Domain.Entities.OrderDomain;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);

            // If we will use PostgreSql db then below in the name convention code
            // how it create the db table name and table properties name.
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName()?.Underscore());

                var ecommerceObjectIdentifier = StoreObjectIdentifier.Table(entity.GetTableName()?.Underscore()!, entity.GetSchema());

                // Replace column names
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName(ecommerceObjectIdentifier)?.Underscore());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName()?.Underscore());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName()?.Underscore());
                }
            }
        }
    }
}
