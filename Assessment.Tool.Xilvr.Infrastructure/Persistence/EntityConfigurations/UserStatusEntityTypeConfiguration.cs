using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Entity configurations for user status
/// </summary>
public class UserStatusEntityTypeConfiguration : IEntityTypeConfiguration<UserStatus>
{

    /// <summary>
    /// Configures the entity of type <typeparamref name="UserStatus" />.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<UserStatus> builder)
    {
        builder.ToTable("user_status", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("user_status_id")
            .ValueGeneratedNever();
        builder.Property(x => x.Status)
          .UsePropertyAccessMode(PropertyAccessMode.Field)
          .HasColumnName("status").HasColumnType($"character varying({Constants.USER_STATUS_LENGTH})")
          .IsRequired();
        builder.Property(x => x.IsActive)
          .UsePropertyAccessMode(PropertyAccessMode.Field)
          .HasColumnName("active").HasMaxLength(Constants.USER_STATUS_LENGTH);
    }
}
