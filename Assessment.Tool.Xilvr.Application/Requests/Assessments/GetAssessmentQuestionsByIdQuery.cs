using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.Assessments;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.Assessments;

/// <summary>
/// Query class for GetAssessmentQuestionsByIdQuery
/// </summary>
public class GetAssessmentQuestionsByIdQuery : IQuery<ApiResponse<AssessmentDetailsDto>>
{
    public int Id { get; set; }
}

/// <summary>
/// Handler class for GetAssessmentQuestionsByIdQuery
/// </summary>
public class GetAssessmentQuestionsByIdQueryHandler : IQueryHandler<GetAssessmentQuestionsByIdQuery, ApiResponse<AssessmentDetailsDto>>
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
    /// Assessment Service
    /// </summary>
    private readonly IAssessmentService _assessmentService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAssessmentQuestionsByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public GetAssessmentQuestionsByIdQueryHandler(IApplicationDbContext dbContext, ITokenService tokenService,
        IAssessmentService assessmentService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
        Ensure.IsNotNull(assessmentService, nameof(assessmentService));
        _assessmentService = assessmentService;
    }

    /// <summary>
    /// The handle method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<AssessmentDetailsDto>> Handle(GetAssessmentQuestionsByIdQuery request, CancellationToken cancellationToken)
    {
        var assessment = await _dbContext.Assessments
            .Include(a => a.Questions)
        .Where(a => a.Id == request.Id)
        .Select(a => new AssessmentDetailsDto
        {
            Id = a.Id,
            Name = a.Name,
            TotalMarks = a.TotalMarks,
            Questions = a.Questions.Select(q => new QuestionsDto
            {
                Id = q.Id,
                Text = q.Text,
                QuestionType = q.QuestionType,
                Options = q.Options,
                Answer = q.Answer,
                Points = q.Points
            }).ToList()
        })
        .FirstOrDefaultAsync(cancellationToken);

        if (assessment == null)
        {
            throw new XilvrException(ExceptionCode.NotFound, Constants.NO_DATA);
        }

        return new ApiResponse<AssessmentDetailsDto>(assessment, Constants.SUCCESS_MSG);
    }
}
