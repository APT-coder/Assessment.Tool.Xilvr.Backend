using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentScores;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Shared.Constants;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessmentScores;

/// <summary>
/// Query class for GetAllEmployeeScoresByScheduledIdQuery
/// </summary>
public class GetAllEmployeeScoresByScheduledIdQuery : IQuery<ApiResponse<List<ScheduledAssessmentScoreDetailsDto>>>
{
    public int ScheduledAssessmentId { get; set; }

    public bool? IsGenerateReport { get; set; }
}

/// <summary>
/// Handler class for GetAllEmployeeScoresByScheduledIdQuery
/// </summary>
public class GetAllEmployeeScoresByScheduledIdQueryHandler : IQueryHandler<GetAllEmployeeScoresByScheduledIdQuery, ApiResponse<List<ScheduledAssessmentScoreDetailsDto>>>
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
    /// Scheduled Assessment Service
    /// </summary>
    private readonly IScheduledAssessmentService _scheduledAssessmentService;

    /// <summary>
    /// Document Service
    /// </summary>
    private readonly IDocumentService _documentService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllEmployeeScoresByScheduledIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public GetAllEmployeeScoresByScheduledIdQueryHandler(IApplicationDbContext dbContext, ITokenService tokenService,
        IScheduledAssessmentService scheduledAssessmentService,
        IDocumentService documentService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
        Ensure.IsNotNull(scheduledAssessmentService, nameof(scheduledAssessmentService));
        _scheduledAssessmentService = scheduledAssessmentService;
        Ensure.IsNotNull(documentService, nameof(_documentService));
        _documentService = documentService;
    }

    /// <summary>
    /// The handle method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<List<ScheduledAssessmentScoreDetailsDto>>> Handle(GetAllEmployeeScoresByScheduledIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _scheduledAssessmentService.GetScoreDetailsByScheduledAssessmentIdAsync(request.ScheduledAssessmentId, cancellationToken);

        if (request.IsGenerateReport == true)
        {
            var meta = await _documentService.GenerateAssessmentReportAsExcel(response, request.ScheduledAssessmentId);

            return new ApiResponse<List<ScheduledAssessmentScoreDetailsDto>>(
                data: response,
                message: Constants.SUCCESS_MSG,
                meta: meta
            );
        }

        return new ApiResponse<List<ScheduledAssessmentScoreDetailsDto>>(response, Constants.SUCCESS_MSG);
    }
}
