using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain;

namespace MyFinance.Persistence.EntityTypeConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
