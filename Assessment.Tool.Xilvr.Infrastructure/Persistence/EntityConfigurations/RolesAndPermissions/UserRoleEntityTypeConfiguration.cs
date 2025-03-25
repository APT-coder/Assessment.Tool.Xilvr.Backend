using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations.RolesAndPermissions;

public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_role", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("user_role_id");
        builder.Property(x => x.RoleId).HasColumnName("role_id");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.SortOrder).HasColumnName("sort_order");
    }
}
