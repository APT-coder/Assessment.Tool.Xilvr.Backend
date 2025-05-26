using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.Assessments;

/// <summary>
/// Query class for AddQuestionToAssessmentByIdCommand
/// </summary>
public class AddQuestionToAssessmentByIdCommand : IQuery<ApiResponse<bool>>
{
    public int Id { get; set; }

    public List<Question> Questions { get; set; } = default!;
}

/// <summary>
/// Handler class for AddQuestionToAssessmentByIdCommand
/// </summary>
public class AddQuestionToAssessmentByIdCommandHandler : IQueryHandler<AddQuestionToAssessmentByIdCommand, ApiResponse<bool>>
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
    /// Initializes a new instance of the <see cref="AddQuestionToAssessmentByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public AddQuestionToAssessmentByIdCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService,
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
    public async Task<ApiResponse<bool>> Handle(AddQuestionToAssessmentByIdCommand request, CancellationToken cancellationToken)
    {
        var assessment = await _dbContext.Assessments
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (assessment is null)
        {
            throw new XilvrException(ExceptionCode.UnprocessableEntity, Constants.UPDATE_FAILED);
        }

        var questions = await _assessmentService.SaveQuestions(request.Questions, cancellationToken);
        var email = _tokenService.TryGetEmailFromToken();

        assessment.Questions.Clear();
        foreach (var question in questions)
        {
            assessment.Questions.Add(question);
        }

        assessment.UpdatedDateTime = DateTime.UtcNow;
        assessment.UpdatedBy = email;

        _dbContext.Assessments.Update(assessment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
