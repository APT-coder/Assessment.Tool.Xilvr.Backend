using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations.RolesAndPermissions;

public class PermissionGroupEntityTypeConfiguration : IEntityTypeConfiguration<PermissionGroup>
{
    public void Configure(EntityTypeBuilder<PermissionGroup> builder)
    {
        builder.ToTable("permission_group", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("permission_group_id");
        builder.Property(x => x.Name).HasColumnName("permission_group_name");
    }
}
