using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.IdentityModel.Tokens;

namespace Assessment.Tool.Xilvr.Application.Requests.Assessments;

/// <summary>
/// Query class for CreateAssessmentCommand
/// </summary>
public class CreateAssessmentCommand : IQuery<ApiResponse<bool>>
{
    public string Name { get; set; } = default!;

    public int TotalMarks { get; set; }

    public List<Question> Questions { get; set; } = default!;
}

/// <summary>
/// Handler class for CreateAssessmentCommand
/// </summary>
public class CreateAssessmentCommandHandler : IQueryHandler<CreateAssessmentCommand, ApiResponse<bool>>
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
    /// Initializes a new instance of the <see cref="CreateAssessmentCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public CreateAssessmentCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService,
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
    public async Task<ApiResponse<bool>> Handle(CreateAssessmentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || (request.Questions.IsNullOrEmpty()))
        {
            throw new XilvrException(ExceptionCode.BadRequest, Constants.CREATE_FAILED);
        }

        var questions = await _assessmentService.SaveQuestions(request.Questions, cancellationToken);
        var email = _tokenService.TryGetEmailFromToken();

        var assessment = new Domain.Entities.Assessment
        {
            Name = request.Name,
            Questions = questions,
            CreatedBy = email,
            CreatedDateTime = DateTime.UtcNow,
        };

        _dbContext.Assessments.Add(assessment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
