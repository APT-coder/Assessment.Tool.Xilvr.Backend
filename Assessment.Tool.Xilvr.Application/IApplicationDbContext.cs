using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application;

public interface IApplicationDbContext
{
    #region DB Sets

    /// <summary>
    /// Gets or sets the user
    /// </summary>
    DbSet<User> Users { get; set; }

    /// <summary>
    /// Specifies the Employee entity
    /// </summary>
    DbSet<Employee> Employees { get; set; }

    /// <summary>
    /// Specifies the roles collection
    /// </summary>
    DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Specifies the permission collection
    /// </summary>
    DbSet<Permission> Permissions { get; set; }

    /// <summary>
    /// Specifies the permission group collections
    /// </summary>
    DbSet<PermissionGroup> PermissionGroups { get; set; }

    /// <summary>
    /// Specifies the role permission collection
    /// </summary>
    DbSet<RolePermission> RolePermissions { get; set; }

    /// <summary>
    /// Specifies the user roles collection
    /// </summary>
    DbSet<UserRole> UserRoles { get; set; }

    /// <summary>
    /// Specifies the user statuses collection
    /// </summary>
    DbSet<UserStatus> UserStatuses { get; set; }

    /// <summary>
    /// Specifies the batches collection
    /// </summary>
    DbSet<Batch> Batches { get; set; }

    /// <summary>
    /// Specifies the questions collection
    /// </summary>
    DbSet<Question> Questions { get; set; }

    /// <summary>
    /// Specifies the scheduled assessments collection
    /// </summary>
    DbSet<ScheduledAssessment> ScheduledAssessments { get; set; }

    /// <summary>
    /// Specifies the scheduled assessment answers collection
    /// </summary>
    DbSet<ScheduledAssessmentAnswer> ScheduledAssessmentsAnswers { get; set; }

    /// <summary>
    /// Specifies the scheduled assessment scores collection
    /// </summary>
    DbSet<ScheduledAssessmentScore> ScheduledAssessmentsScores { get; set; }

    #endregion
}
