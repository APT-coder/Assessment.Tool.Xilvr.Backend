using Assessment.Tool.Xilvr.Domain.Aggregates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Assessment.Tool.Xilvr.Shared.Constants;
using Newtonsoft.Json;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Entity configurations for Employee
/// </summary>
public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="Employee" />.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employee", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("employee_id")
            .ValueGeneratedOnAdd();
        builder.Property(c => c.UserId)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("user_id")
            .IsRequired();
        builder.Property(x => x.BatchIds)
            .HasConversion(
                c => JsonConvert.SerializeObject(c),
                c => JsonConvert.DeserializeObject<List<string>>(c))
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("batch_ids")
            .IsRequired(false);
        builder.Property(c => c.CreatedDateTime)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("created_date").IsRequired();
        builder.Property(c => c.UpdatedDateTime)
           .UsePropertyAccessMode(PropertyAccessMode.Field)
           .HasColumnName("updated_date");
        builder.Property(c => c.CreatedBy)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("created_by").HasMaxLength(Constants.CREATED_BY_AND_UPDATED_BY_LENGTH);
        builder.Property(c => c.UpdatedBy)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("updated_by").HasMaxLength(Constants.CREATED_BY_AND_UPDATED_BY_LENGTH);
        builder.Property(e => e.Designation)
            .HasColumnName("designation")
            .HasMaxLength(100);
        builder.Property(c => c.IsActive)
             .UsePropertyAccessMode(PropertyAccessMode.Field)
             .HasColumnName("active").IsRequired();

        builder.HasOne(x => x.User)
              .WithMany()
              .HasForeignKey(x => x.UserId)
              .HasPrincipalKey(x => x.Id);
    }
}
