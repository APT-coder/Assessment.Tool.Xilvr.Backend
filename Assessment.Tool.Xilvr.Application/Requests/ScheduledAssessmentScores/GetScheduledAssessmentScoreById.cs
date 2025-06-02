using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.Employees;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentScores;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessmentScores;

/// <summary>
/// Query class for GetScheduledAssessmentScoreByIdQuery
/// </summary>
public class GetScheduledAssessmentScoreByIdQuery : IQuery<ApiResponse<ScheduledAssessmentScoreDto>>
{
    public int ScheduledAssessmentId { get; set; }
}

/// <summary>
/// Handler class for GetScheduledAssessmentScoreByIdQuery
/// </summary>
public class GetScheduledAssessmentScoreByIdQueryHandler : IQueryHandler<GetScheduledAssessmentScoreByIdQuery, ApiResponse<ScheduledAssessmentScoreDto>>
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
    /// Initializes a new instance of the <see cref="GetScheduledAssessmentScoreByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public GetScheduledAssessmentScoreByIdQueryHandler(IApplicationDbContext dbContext, ITokenService tokenService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
    }

    /// <summary>
    /// The handle method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<ScheduledAssessmentScoreDto>> Handle(GetScheduledAssessmentScoreByIdQuery request, CancellationToken cancellationToken)
    {
        var scheduledAssessment = await _dbContext.ScheduledAssessments
            .Include(s => s.Assessment)
            .Include(s => s.Batch)
            .FirstOrDefaultAsync(s => s.Id == request.ScheduledAssessmentId, cancellationToken);

        if (scheduledAssessment == null)
        {
            throw new XilvrException(ExceptionCode.NotFound, Constants.NO_DATA);
        }

        var employeesInBatch = _dbContext.Employees
            .AsEnumerable()
            .Where(e => e.BatchIds.Contains(scheduledAssessment.BatchId.ToString()))
            .ToList();

        var answers = await _dbContext.ScheduledAssessmentsAnswers
            .Where(a => a.ScheduledAssessmentId == request.ScheduledAssessmentId)
            .Include(a => a.Employee)
            .ToListAsync(cancellationToken);

        var storedScores = await _dbContext.ScheduledAssessmentsScores
            .Where(s => s.ScheduledAssessmentId == request.ScheduledAssessmentId)
            .Include(s => s.Employee).ThenInclude(e => e.User)
            .ToListAsync(cancellationToken);

        var attendees = employeesInBatch
        .Select(emp =>
        {
            var stored = storedScores.FirstOrDefault(s => s.EmployeeId == emp.Id);
            if (stored != null)
            {
                return new EmployeeScoreDto
                {
                    EmployeeId = stored.EmployeeId,
                    EmployeeName = stored.Employee.User.FirstName + " " + stored.Employee.User.LastName,
                    Score = stored.Score
                };
            }

            var employeeAnswers = answers.Where(a => a.EmployeeId == emp.Id).ToList();
            if (employeeAnswers.Any())
            {
                return new EmployeeScoreDto
                {
                    EmployeeId = emp.Id,
                    EmployeeName = emp.User.FirstName + " " + emp.User.LastName,
                    Score = employeeAnswers.Sum(a => a.Score)
                };
            }

            return null;
        })
        .Where(x => x != null)
        .ToList();

        var attendeeIds = attendees.Select(a => a.EmployeeId).ToHashSet();

        var absentees = employeesInBatch
            .Where(e => !attendeeIds.Contains(e.Id))
            .Select(e => new EmployeeDto
            {
                EmployeeId = e.Id,
                EmployeeName = e.User.FirstName + " " + e.User.LastName,
            }).ToList();

        var totalCount = employeesInBatch.Count;
        var attendedCount = attendees.Count;

        var attendance = $"{attendedCount}/{totalCount}";

        var dto = new ScheduledAssessmentScoreDto
        {
            Attendees = attendees,
            Absentees = absentees,
            Attendance = attendance
        };

        return new ApiResponse<ScheduledAssessmentScoreDto>(dto, Constants.SUCCESS_MSG);
    }
}
