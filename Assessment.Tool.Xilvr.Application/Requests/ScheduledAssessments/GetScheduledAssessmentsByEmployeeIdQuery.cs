using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessments;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessments;

/// <summary>
/// Query class for GetScheduledAssessmentsByEmployeeIdQuery
/// </summary>
public class GetScheduledAssessmentsByEmployeeIdQuery : IQuery<ApiResponse<List<ScheduledAssessmentListDto>>>
{
    public long? EmployeeId { get; set; }

    public bool? ForEmployee { get; set; } = default!;

    public int Skip { get; set; }

    public int Take { get; set; }
}

/// <summary>
/// Handler class for GetScheduledAssessmentsByEmployeeIdQuery
/// </summary>
public class GetScheduledAssessmentsByEmployeeIdQueryHandler : IQueryHandler<GetScheduledAssessmentsByEmployeeIdQuery, ApiResponse<List<ScheduledAssessmentListDto>>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Token Service
    /// </summary>
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Employee Repository
    /// </summary>
    private readonly IEmployeeRepository _employeeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetScheduledAssessmentsByEmployeeIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public GetScheduledAssessmentsByEmployeeIdQueryHandler(IApplicationDbContext dbContext, ITokenService tokenService,
        IEmployeeRepository employeeRepository)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
        Ensure.IsNotNull(employeeRepository, nameof(_employeeRepository));
        _employeeRepository = employeeRepository;
    }

    /// <summary>
    /// The handle method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<List<ScheduledAssessmentListDto>>> Handle(GetScheduledAssessmentsByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        IQueryable<ScheduledAssessment> query = _dbContext.ScheduledAssessments
        .Include(s => s.Batch)
        .Include(s => s.Assessment);

        if (request.EmployeeId.HasValue && request.ForEmployee.HasValue)
        {
            var employee = await _employeeRepository.GetEmployeeById(request.EmployeeId.Value, cancellationToken);

            if (employee == null || !employee.IsActive)
            {
                throw new XilvrException(ExceptionCode.BadRequest, Constants.NO_DATA);
            }

            if (request.ForEmployee.Value)
            {
                query = query.Where(s => employee.BatchIds.Contains(s.BatchId.ToString()));
            }
            else
            {
                query = query.Where(s => s.CreatedBy == employee.User.Email.EmailId);
            }
        }

        var response = await query
            .Select(s => new ScheduledAssessmentListDto
            {
                Id = s.Id,
                BatchName = s.Batch.Name,
                AssessmentName = s.Assessment.Name,
                AssessmentDuration = s.AssessmentDuration,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                AssessmentStatus = s.AssessmentStatus,
                IsEvaluated = request.ForEmployee == true
                    ? _dbContext.ScheduledAssessmentsScores
                        .Where(a => a.ScheduledAssessmentId == s.Id && a.EmployeeId == request.EmployeeId)
                        .Select(a => (bool?)a.IsEvaluated)
                        .FirstOrDefault()
                    : null
            })
            .Skip(request.Skip)
            .Take(request.Take)
            .ToListAsync(cancellationToken);

        return new ApiResponse<List<ScheduledAssessmentListDto>>(response, Constants.SUCCESS_MSG);
    }
}
