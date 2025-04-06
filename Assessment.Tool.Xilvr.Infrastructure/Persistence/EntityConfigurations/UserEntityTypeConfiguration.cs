using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Assessment.Tool.Xilvr.Shared.Constants;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Entity configurations for user
/// </summary>
public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="User" />.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("user_id")
            .ValueGeneratedOnAdd();
        builder.Property(c => c.UuId)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("uid")
            .IsRequired();
        builder.Property(c => c.ProfileImageUrl)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("profile_image_url")
            .IsRequired(false)
            .HasMaxLength(Constants.PROFILE_IMAGE_URL_LENGTH);
        builder.Property(c => c.UserStatusId)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("user_status_id")
            .IsRequired();
        builder.OwnsOne(c => c.Email, emailBuilder =>
        {
            emailBuilder.Property(a => a.EmailId).IsRequired().HasColumnName("email").HasMaxLength(Constants.EMAIL_LENGTH)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        });
        builder.Property(c => c.FirstName)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("first_name").HasMaxLength(Constants.USER_NAME_LENGTH);
        builder.Property(c => c.LastName)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("last_name")
            .HasMaxLength(Constants.USER_NAME_LENGTH);
        builder.Property(c => c.Password)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("password");
        builder.Property(c => c.Phone)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("phone");
        builder.Property(c => c.LasttPasswordReset)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("password_reset_on");
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

        builder.HasOne(x => x.UserStatus)
            .WithMany()
            .HasForeignKey(x => x.UserStatusId)
            .HasPrincipalKey(x => x.Id);

        builder.HasMany(x => x.Employees)
           .WithOne(x => x.User)
           .HasForeignKey(x => x.UserId);
    }
}
