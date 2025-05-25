using Assessment.Tool.Xilvr.Application.DataSeeders;
using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Infrastructure;

/// <summary>
/// Seeder class for application management
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Seed the data
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        SeedUserStatus(modelBuilder);
        SeedRoles(modelBuilder);
        SeedPermissionGroups(modelBuilder);
    }

    /// <summary>
    /// seed user status
    /// </summary>
    /// <param name="modelBuilder"></param>
    private static void SeedUserStatus(ModelBuilder modelBuilder)
    {
        foreach (var status in UserStatusSeeder.GetData())
        {
            modelBuilder.Entity<UserStatus>().HasData(status);
        }
    }

    /// <summary>
    /// Seeds the roles data
    /// </summary>
    /// <param name="modelBuilder"></param>
    private static void SeedRoles(ModelBuilder modelBuilder)
    {
        var roles = RoleSeeder.GetData();
        modelBuilder.Entity<Role>().HasData(roles);
    }

    /// <summary>
    /// Seeds the permission groups data
    /// </summary>
    /// <param name="modelBuilder"></param>
    private static void SeedPermissionGroups(ModelBuilder modelBuilder)
    {
        var permissionGroups = PermissionGroupSeeder.GetData();
        modelBuilder.Entity<PermissionGroup>().HasData(permissionGroups);
    }
}
