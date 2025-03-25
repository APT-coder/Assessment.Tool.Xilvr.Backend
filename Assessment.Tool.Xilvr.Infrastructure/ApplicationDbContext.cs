using System.Reflection;
using Assessment.Tool.Xilvr.Application;
using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Assessment.Tool.Xilvr.Infrastructure;

/// <summary>
/// DB context for application
/// </summary>
public class ApplicationContext : DbContext, IApplicationDbContext
{
    internal const string DEFAULT_SCHEMA = "xilvr";

    private readonly IMediator _mediator;

    #region DB Sets
    /// <summary>
    /// Gets or sets the user
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Specifies the Employee entity
    /// </summary>
    public DbSet<Employee> Employees { get; set; }

    /// <summary>
    /// Specifies the roles collection
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Specifies the permission collection
    /// </summary>
    public DbSet<Permission> Permissions { get; set; }

    /// <summary>
    /// Specifies the permission group collections
    /// </summary>
    public DbSet<PermissionGroup> PermissionGroups { get; set; }

    /// <summary>
    /// Specifies the role permission collection
    /// </summary>
    public DbSet<RolePermission> RolePermissions { get; set; }

    /// <summary>
    /// Specifies the user roles collection
    /// </summary>
    public DbSet<UserRole> UserRoles { get; set; }

    /// <summary>
    /// Specifies the user statuses collection
    /// </summary>
    public DbSet<UserStatus> UserStatuses { get; set; }

    /// <summary>
    /// Specifies the batches collection
    /// </summary>
    public DbSet<Batch> Batches { get; set; }

    /// <summary>
    /// Specifies the questions collection
    /// </summary>
    public DbSet<Question> Questions { get; set; }

    /// <summary>
    /// Specifies the scheduled assessments collection
    /// </summary>
    public DbSet<ScheduledAssessment> ScheduledAssessments { get; set; }

    /// <summary>
    /// Specifies the scheduled assessment answers collection
    /// </summary>
    public DbSet<ScheduledAssessmentAnswer> ScheduledAssessmentsAnswers { get; set; }

    /// <summary>
    /// Specifies the scheduled assessment scores collection
    /// </summary>
    public DbSet<ScheduledAssessmentScore> ScheduledAssessmentsScores { get; set; }

    #endregion

    /// <summary>
    /// Specifies whether has active transactions or not.
    /// </summary>
    public bool HasActiveTransaction => _currentTransaction != null;

    /// <summary>
    /// Holds the transaction object
    /// </summary>
    private IDbContextTransaction? _currentTransaction;

    /// <summary>
    /// Gets the current transaction
    /// </summary>
    /// <returns></returns>
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction ??
        throw new ArgumentNullException(nameof(IDbContextTransaction));

    /// <summary>
    /// Constructor for Application Db Context
    /// </summary>
    /// <param name="options"></param>
    /// <param name="mediator"></param>
    public ApplicationContext(DbContextOptions<ApplicationContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    /// <summary>
    /// Method for creating model
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
        base.OnModelCreating(modelBuilder);

        // Applies configurations from all IEntityTypeConfiguration<TEntity> in the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(), t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
            )
        );

        //modelBuilder.SeedData();
    }

    /// <summary>
    /// Method to save changes in entities
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// begin sql transaction
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);

        return _currentTransaction;
    }

    /// <summary>
    /// Commits a transaction
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    /// <summary>
    /// Rollback transaction changes
    /// </summary>
    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    /// <summary>
    /// Get database facade
    /// </summary>
    /// <returns></returns>
    public DatabaseFacade GetDatabase()
    {
        return Database;
    }
}
