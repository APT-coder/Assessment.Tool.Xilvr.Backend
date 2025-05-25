using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using System.Diagnostics.CodeAnalysis;

namespace Assessment.Tool.Xilvr.Application.DataSeeders;

/// <summary>
/// Defines the <see cref="PermissionGroupSeeder" />.
/// </summary>
[ExcludeFromCodeCoverage]
public static class PermissionGroupSeeder
{
    /// <summary>
    /// Gets the permission group seed data
    /// </summary>
    /// <returns></returns>
    public static List<PermissionGroup> GetData()
    {
        return new List<PermissionGroup>
        {
            new PermissionGroup { Id = 1, Name = "Trainee Listing - Trainee Listing" },
            new PermissionGroup { Id = 2, Name = "Assessments - View Assessments Listing" },
            new PermissionGroup { Id = 3, Name = "Assessments - Schedule Assessment" },
            new PermissionGroup { Id = 4, Name = "Assessments - Question Bank" },
            new PermissionGroup { Id = 5, Name = "Assessments - Administration" },
            new PermissionGroup { Id = 6, Name = "Assessments - Reports" },
            new PermissionGroup { Id = 7, Name = "Assessments - Evaluate Assessment" },
            new PermissionGroup { Id = 8, Name = "Configuration Management - Manage User Roles" },
            new PermissionGroup { Id = 9, Name = "Configuration Management - Manage Roles and Permissions"}
        };
    }
}
