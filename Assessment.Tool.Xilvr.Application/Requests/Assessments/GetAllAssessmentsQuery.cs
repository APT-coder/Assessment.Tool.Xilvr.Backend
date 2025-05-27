using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos;
using Assessment.Tool.Xilvr.Application.Dtos.Assessments;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.Assessments;

/// <summary>
/// Query class for GetAllAssessmentsQuery
/// </summary>
public class GetAllAssessmentsQuery : PaginationDto, IQuery<ApiResponse<List<AssessmentListDto>>>
{
    public string? Name { get; set; } = default!;
}

/// <summary>
/// Handler class for GetAllAssessmentsQuery
/// </summary>
public class GetAllAssessmentsQueryHandler : IQueryHandler<GetAllAssessmentsQuery, ApiResponse<List<AssessmentListDto>>>
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
    /// Initializes a new instance of the <see cref="GetAllAssessmentsQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public GetAllAssessmentsQueryHandler(IApplicationDbContext dbContext, ITokenService tokenService,
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
    public async Task<ApiResponse<List<AssessmentListDto>>> Handle(GetAllAssessmentsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Assessments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var searchTerm = request.Name.ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(searchTerm));
        }

        var assessments = await query
            .OrderBy(a => a.Id)
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(a => new AssessmentListDto
            {
                Id = a.Id,
                Name = a.Name,
                TotalMarks = a.TotalMarks
            })
            .ToListAsync(cancellationToken);

        return new ApiResponse<List<AssessmentListDto>>(assessments, Constants.SUCCESS_MSG);
    }
}
