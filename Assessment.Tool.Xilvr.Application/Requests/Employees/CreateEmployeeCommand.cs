using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Assessment.Tool.Xilvr.Shared.Constants;

namespace Assessment.Tool.Xilvr.Application.Requests.Employees;

/// <summary>
/// Query class for creating employee
/// </summary>
public class CreateEmployeeCommand : IQuery<ApiResponse<bool>>
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public short UserStatusId { get; set; }

    public string Phone { get; set; } = default!;

    public string ProfileImageUrl { get; set; } = default!;

    public string Designation { get; set; } = default!;

    public List<string>? BatchIds { get; set; } = new List<string>();
}

/// <summary>
/// Handler class for CreateEmployeeCommand
/// </summary>
public class CreateEmployeeCommandHandler : IQueryHandler<CreateEmployeeCommand, ApiResponse<bool>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Employee repository
    /// </summary>
    private readonly IEmployeeRepository _employeeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEmployeeCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public CreateEmployeeCommandHandler(IApplicationDbContext dbContext, IEmployeeRepository employeeRepository)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(employeeRepository, nameof(employeeRepository));
        _employeeRepository = employeeRepository;
    }

    /// <summary>
    /// The hande method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<bool>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByEmailAsync(request.Email, cancellationToken);
        var userStatus = UserStatus.SetFrom((UserStatusValues)request.UserStatusId);
        if (employee == null)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            employee = Employee.Create(request.ProfileImageUrl, request.FirstName, request.LastName,
                request.Email, request.BatchIds, request.Phone, request.FirstName, hashedPassword,
                request.Designation, false, Shared.Enum.UserProvider.Local, "0");
            employee.ActivateUser();

            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
        }
        return new ApiResponse<bool>(false, Constants.CREATE_FAILED);
    }
}
