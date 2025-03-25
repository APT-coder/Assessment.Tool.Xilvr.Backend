using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations.RolesAndPermissions;

public class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permission", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("permission_id");
        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("permission_name")
            .HasMaxLength(200);
        builder.Property(x => x.PermissionKey)
            .IsRequired(false)
            .HasColumnName("permission_key")
            .HasMaxLength(250);
        builder.Property(x => x.PermissionGroupId)
            .HasColumnName("permission_group_id")
            .IsRequired(false);
        builder.Property(x => x.IsInternalPermission).HasColumnName("is_internal_permission");
    }
}
