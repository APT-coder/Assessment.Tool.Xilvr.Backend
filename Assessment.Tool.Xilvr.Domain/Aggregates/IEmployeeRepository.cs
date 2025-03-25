using Assessment.Tool.Xilvr.Base.Domain;

namespace Assessment.Tool.Xilvr.Domain.Aggregates;

/// <summary>
/// Provides the employee requisition repository contracts.
/// </summary>
public interface IEmployeeRepository : IRepository<Employee>
{
    /// <summary>
    /// Get employee based on email id
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the employee detail by id
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    Task<Employee?> GetEmployeeById(long employeeId);

    /// <summary>
    /// Gets the employee by User Guid
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    Task<Employee?> GetEmployeeByUserGuidAsync(Guid guid);

    /// <summary>
    /// Gets the employee by user identifier asynchronous.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns></returns>
    Task<Employee?> GetEmployeeByUserIdAsync(long userId);

    /// <summary>
    /// Add employee
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    Employee AddEmployee(Employee employee);

    /// <summary>
    /// Updates an existing employee
    /// </summary>
    /// <param name="employee"></param>
    void Update(Employee employee);
}
