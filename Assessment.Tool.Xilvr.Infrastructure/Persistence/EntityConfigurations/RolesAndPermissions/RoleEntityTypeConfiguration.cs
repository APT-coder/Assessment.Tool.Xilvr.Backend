using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations.RolesAndPermissions;

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("role", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("role_id");
        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("role_name")
            .HasMaxLength(200);
        builder.Property(x => x.RoleInternalName)
            .IsRequired(false)
            .HasColumnName("role_internal_name")
            .HasMaxLength(250);
        builder.Property(x => x.IsDefaultRole).HasColumnName("is_default_role");
        builder.Property(x => x.IsSystemRole).HasColumnName("is_system_role");
    }
}
