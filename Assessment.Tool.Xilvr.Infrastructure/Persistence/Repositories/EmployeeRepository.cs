using Assessment.Tool.Xilvr.Base.Domain.SeedWork;
using Assessment.Tool.Xilvr.Domain.Aggregates;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.Repositories;

/// <summary>
/// Class implements the Employee repository contracts.
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    public IUnitOfWork UnitOfWork => throw new NotImplementedException();

    public Employee AddEmployee(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Employee?> GetEmployeeById(long employeeId)
    {
        throw new NotImplementedException();
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
