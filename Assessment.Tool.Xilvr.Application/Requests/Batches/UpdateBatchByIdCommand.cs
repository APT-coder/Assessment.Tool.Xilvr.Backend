using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.Batches;

/// <summary>
/// Query class for UpdateBatchByIdCommand
/// </summary>
public class UpdateBatchByIdCommand : IQuery<ApiResponse<bool>>
{
    public int Id { get; set; }

    public string? Name { get; set; } = default!;

    public bool? IsActive { get; set; }
}

/// <summary>
/// Handler class for UpdateBatchByIdCommand
/// </summary>
public class UpdateBatchByIdCommandHandler : IQueryHandler<UpdateBatchByIdCommand, ApiResponse<bool>>
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
    /// Initializes a new instance of the <see cref="UpdateBatchByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public UpdateBatchByIdCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService)
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
    public async Task<ApiResponse<bool>> Handle(UpdateBatchByIdCommand request, CancellationToken cancellationToken)
    {
        var batch = await _dbContext.Batches
            .FirstOrDefaultAsync(b => b.Id == request.Id && b.IsActive == true, cancellationToken);

        if (batch == null)
        {
            throw new XilvrException(ExceptionCode.NotFound, "Batch not found");
        }

        var email = _tokenService.TryGetEmailFromToken();

        batch.Name = request.Name ?? batch.Name;
        batch.IsActive = request.IsActive ?? batch.IsActive;
        batch.UpdatedDateTime = DateTime.UtcNow;
        batch.UpdatedBy = email;

        _dbContext.Batches.Update(batch);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
