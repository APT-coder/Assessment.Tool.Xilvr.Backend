using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.AspNetCore.Http;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessments;

/// <summary>
/// Query class for ParseQuestionsDocumentCommand
/// </summary>
public class ParseQuestionsDocumentCommand : IQuery<ApiResponse<List<Question>>>
{
    public IFormFile File { get; set; } = default!;
}

/// <summary>
/// Handler class for ParseQuestionsDocumentCommand
/// </summary>
public class ParseQuestionsDocumentCommandHandler : IQueryHandler<ParseQuestionsDocumentCommand, ApiResponse<List<Question>>>
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
    /// Document Service
    /// </summary>
    private readonly IDocumentService _documentService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParseQuestionsDocumentCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public ParseQuestionsDocumentCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService,
        IDocumentService documentService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
        Ensure.IsNotNull(documentService, nameof(documentService));
        _documentService = documentService;
    }

    /// <summary>
    /// The handle method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<List<Question>>> Handle(ParseQuestionsDocumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var questions = await _documentService.ParseQuestionsFromFile(request.File);
            return new ApiResponse<List<Question>>(questions, Constants.SUCCESS_MSG);
        }
        catch (Exception ex)
        {
            throw new XilvrException(ExceptionCode.UnprocessableEntity, ex.Message);
        }
    }
}
