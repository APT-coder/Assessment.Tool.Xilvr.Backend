using Assessment.Tool.Xilvr.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Assessment.Tool.Xilvr.Shared.Constants;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations;

public class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("batch", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(b => b.Id);
        builder.Property(c => c.Id)
            .HasColumnName("batch_id")
            .ValueGeneratedOnAdd();
        builder.Property(b => b.Name)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("batch_name")
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(b => b.IsActive)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_active")
            .IsRequired(false);
        builder.Property(c => c.CreatedDateTime)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("created_date")
            .IsRequired();
        builder.Property(c => c.UpdatedDateTime)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("updated_date");
        builder.Property(c => c.CreatedBy)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("created_by").HasMaxLength(Constants.CREATED_BY_AND_UPDATED_BY_LENGTH);
        builder.Property(c => c.UpdatedBy)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("updated_by").HasMaxLength(Constants.CREATED_BY_AND_UPDATED_BY_LENGTH);
    }
}
