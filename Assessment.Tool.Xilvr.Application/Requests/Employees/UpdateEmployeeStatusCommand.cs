using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Assessment.Tool.Xilvr.Application.Requests.Employees;

/// <summary>
/// Query class for UpdateEmployeeStatusCommand
/// </summary>
public class UpdateEmployeeStatusCommand : IQuery<ApiResponse<bool>>
{
    [JsonIgnore]
    public long EmployeeId { get; set; }

    public bool IsActive { get; set; }

    public short UserStatusId { get; set; }
}

/// <summary>
/// handler for UpdateEmployeeStatusCommand
/// </summary>
public class UpdateEmployeeStatusCommandHandler : IQueryHandler<UpdateEmployeeStatusCommand, ApiResponse<bool>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Employee Repository
    /// </summary>
    private readonly IEmployeeRepository _employeeRepository;

    /// <summary>
    /// Constructor for UpdateEmployeeStatusCommandHandler
    /// </summary>
    public UpdateEmployeeStatusCommandHandler(IApplicationDbContext dbContext, IEmployeeRepository employeeRepository)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        Ensure.IsNotNull(employeeRepository, nameof(employeeRepository));

        _dbContext = dbContext;
        _employeeRepository = employeeRepository;
    }

    /// <summary>
    /// handle method for UpdateEmployeeStatusCommand
    /// </summary>
    public async Task<ApiResponse<bool>> Handle(UpdateEmployeeStatusCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeById(request.EmployeeId, cancellationToken);
        if (employee == null)
            return new ApiResponse<bool>(false, Constants.NO_DATA);

        // Update IsActive and UserStatus
        employee.IsActive = request.IsActive;
        var userStatus = await _dbContext.UserStatuses
            .FirstOrDefaultAsync(x => x.Id == request.UserStatusId, cancellationToken);
        if (userStatus == null)
        {
            throw new XilvrException(ExceptionCode.UnprocessableEntity, "Invalid User Status code");
        }
        employee.User.SetUserStatus(userStatus);

        _dbContext.Employees.Update(employee);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
