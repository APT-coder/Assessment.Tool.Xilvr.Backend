using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations.RolesAndPermissions;

public class RolePermissionEntityTypeConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permission", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("role_permission_id");
        builder.Property(x => x.RoleId).HasColumnName("role_id");
        builder.Property(x => x.PermissionId).HasColumnName("permission_id");
    }
}
