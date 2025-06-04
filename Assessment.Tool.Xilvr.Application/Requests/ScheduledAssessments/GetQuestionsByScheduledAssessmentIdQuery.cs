using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessments;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessments;

/// <summary>
/// Query class for GetQuestionsByScheduledAssessmentIdQuery
/// </summary>
public class GetQuestionsByScheduledAssessmentIdQuery : IQuery<ApiResponse<List<QuestionDto>>>
{
    public int ScheduledAssessmentId { get; set; }

    public long EmployeeId { get; set; }
}

/// <summary>
/// Handler class for GetQuestionsByScheduledAssessmentIdQuery
/// </summary>
public class GetQuestionsByScheduledAssessmentIdQueryHandler : IQueryHandler<GetQuestionsByScheduledAssessmentIdQuery, ApiResponse<List<QuestionDto>>>
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
    /// Initializes a new instance of the <see cref="GetQuestionsByScheduledAssessmentIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public GetQuestionsByScheduledAssessmentIdQueryHandler(IApplicationDbContext dbContext, ITokenService tokenService)
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
    public async Task<ApiResponse<List<QuestionDto>>> Handle(GetQuestionsByScheduledAssessmentIdQuery request, CancellationToken cancellationToken)
    {
        var scheduledAssessment = await _dbContext.ScheduledAssessments
            .Include(s => s.Assessment)
                .ThenInclude(a => a.Questions)
            .FirstOrDefaultAsync(s => s.Id == request.ScheduledAssessmentId, cancellationToken);

        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);

        if (scheduledAssessment == null || employee == null)
        {
            throw new XilvrException(ExceptionCode.BadRequest, Constants.NO_DATA);
        }

        var questions = scheduledAssessment.Assessment.Questions.AsQueryable();

        questions = scheduledAssessment.CanRandomizeQuestion
            ? questions.OrderBy(_ => Guid.NewGuid())
            : questions.OrderBy(q => q.Id);

        var response = questions
            .Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                QuestionType = q.QuestionType,
                Points = q.Points,
                Options = q.Options,
            }).ToList();

        return new ApiResponse<List<QuestionDto>>(response, Constants.SUCCESS_MSG);
    }
}
