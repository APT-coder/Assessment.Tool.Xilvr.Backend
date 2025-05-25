using Assessment.Tool.Xilvr.Base.Domain.SeedWork;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.Repositories;

/// <summary>
/// Class implements the Employee repository contracts.
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    /// <summary>
    /// The application context
    /// </summary>
    public readonly ApplicationContext _applicationContext;

    /// <summary>
    /// The unit of work
    /// </summary>
    public IUnitOfWork UnitOfWork => _applicationContext;

    /// <summary>
    /// Constructor for employee repository
    /// </summary>
    /// <param name="applicationContext"></param>
    public EmployeeRepository(ApplicationContext applicationContext)
    {
        Ensure.IsNotNull(applicationContext, nameof(applicationContext));
        _applicationContext = applicationContext;
    }

    public Employee AddEmployee(Employee employee)
    {
        throw new NotImplementedException();
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var employee = await _applicationContext.Employees
            .Include(e => e.User)
                .ThenInclude(u => u.UserStatus)
            .FirstOrDefaultAsync(e => e.User.Email.EmailId.ToLower() == email.ToLower(), cancellationToken);

        return employee;
    }

    public async Task<Employee?> GetEmployeeById(long employeeId, CancellationToken cancellationToken)
    {
        var employee = await _applicationContext.Employees
            .Include(e => e.User)
                .ThenInclude(u => u.UserStatus)
            .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);

        return employee;
    }

    public Task<Employee?> GetEmployeeByUserGuidAsync(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task<Employee?> GetEmployeeByUserIdAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public void Update(Employee employee)
    {
        throw new NotImplementedException();
    }
}
