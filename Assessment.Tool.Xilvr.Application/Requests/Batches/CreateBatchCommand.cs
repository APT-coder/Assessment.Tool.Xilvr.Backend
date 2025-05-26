using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.Batches;

/// <summary>
/// Query class for CreateBatchCommand
/// </summary>
public class CreateBatchCommand : IQuery<ApiResponse<bool>>
{
    public string Name { get; set; } = default!;
}

/// <summary>
/// Handler class for CreateBatchCommand
/// </summary>
public class CreateBatchCommandHandler : IQueryHandler<CreateBatchCommand, ApiResponse<bool>>
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
    /// Initializes a new instance of the <see cref="CreateBatchCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public CreateBatchCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService)
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
    public async Task<ApiResponse<bool>> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new XilvrException(ExceptionCode.BadRequest, "Batch Name Required");
        }

        var existing = await _dbContext.Batches
            .AnyAsync(b => b.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (existing)
        {
            throw new XilvrException(ExceptionCode.UnprocessableEntity, "Batch already exists");
        }

        var email = _tokenService.TryGetEmailFromToken();

        var newBatch = new Batch
        {
            Name = request.Name,
            CreatedDateTime = DateTime.UtcNow,
            CreatedBy = email,
            IsActive = true
        };

        _dbContext.Batches.Add(newBatch);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
