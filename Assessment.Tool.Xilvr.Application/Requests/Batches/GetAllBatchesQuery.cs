using Assessment.Tool.Xilvr.Application.Dtos.Batches;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.Batches;

/// <summary>
/// Query class for getting all batches
/// </summary>
public class GetAllBatchesQuery : IQuery<ApiResponse<List<BatchDto>>>
{
    public string? IsActive { get; set; }

    public DateOnly? Year { get; set; }

    public string? Search { get; set; }
}

/// <summary>
/// Handler class for GetAllBatchesQuery
/// </summary>
public class GetAllBatchesQueryHandler : IQueryHandler<GetAllBatchesQuery, ApiResponse<List<BatchDto>>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllBatchesQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public GetAllBatchesQueryHandler(IApplicationDbContext dbContext)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
    }

    /// <summary>
    /// The handle method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<List<BatchDto>>> Handle(GetAllBatchesQuery request, CancellationToken cancellationToken)
    {
        bool? isActiveFilter = null;

        if (!string.IsNullOrEmpty(request.IsActive) && bool.TryParse(request.IsActive, out var parsed))
        {
            isActiveFilter = parsed;
        }

        var query = _dbContext.Batches.AsQueryable();

        if (isActiveFilter.HasValue)
        {
            query = query.Where(batch => batch.IsActive == isActiveFilter.Value);
        }

        if (request.Year.HasValue)
        {
            query = query.Where(batch => batch.CreatedDateTime.HasValue &&
                                         batch.CreatedDateTime.Value.Year == request.Year.Value.Year);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            string searchLower = request.Search.ToLower();
            query = query.Where(batch => batch.Name.ToLower().Contains(searchLower));
        }

        var batches = await query
            .Select(batch => new BatchDto
            {
                Id = batch.Id,
                Name = batch.Name,
            })
            .ToListAsync(cancellationToken);

        return new ApiResponse<List<BatchDto>>(batches, Constants.SUCCESS_MSG);
    }
}
