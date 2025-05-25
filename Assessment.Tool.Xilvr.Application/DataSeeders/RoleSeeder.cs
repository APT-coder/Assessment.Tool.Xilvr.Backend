using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;

namespace Assessment.Tool.Xilvr.Application.DataSeeders;

/// <summary>
/// Defines the <see cref="RoleSeeder" />.
/// </summary>
public static class RoleSeeder
{
    /// <summary>
    /// Gets the roles seed data
    /// </summary>
    /// <returns></returns>
    public static List<Role> GetData()
    {
        return new List<Role>
        {
            new Role
            {
                Id = 1,
                Name = "Base Role",
                Description = "Base Role",
                IsDefaultRole = true,
                IsSystemRole = true,
                RoleInternalName = "BaseRole",
            },
            new Role
            {
                Id = 2,
                Name = "Trainee",
                IsDefaultRole = false,
                IsSystemRole = true,
                RoleInternalName = "Trainee",
                Description = "Trainee",
            },
            new Role
            {
                Id = 3,
                Name = "Trainer",
                IsDefaultRole = false,
                IsSystemRole = true,
                RoleInternalName = "Trainer",
                Description = "Trainer",
            },
            new Role
            {
                Id = 4,
                Name = "Trainer Manager",
                IsDefaultRole = false,
                IsSystemRole = true,
                RoleInternalName = "TrainerManager",
                Description = "Trainer Manager",
            },
            new Role
            {
                Id = 5,
                Name = "System Admin",
                IsDefaultRole = false,
                IsSystemRole = true,
                RoleInternalName = "SystemAdmin",
                Description = "System Admin",
            },
        };
    }
}
