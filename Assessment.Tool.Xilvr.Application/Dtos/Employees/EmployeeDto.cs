namespace Assessment.Tool.Xilvr.Application.Dtos.Employees;

/// <summary>
/// Defines the <see cref="EmployeeDto" />.
/// </summary>
public class EmployeeDto
{
    /// <summary>
    /// Gets or sets the employee id.
    /// </summary>
    public long EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the employee name.
    /// </summary>
    public string EmployeeName { get; set; } = default!;
}
