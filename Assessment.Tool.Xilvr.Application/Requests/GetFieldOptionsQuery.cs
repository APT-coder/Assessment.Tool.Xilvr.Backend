using Assessment.Tool.Xilvr.Application.Dtos;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Enums;
using Assessment.Tool.Xilvr.Shared.Constants;
using Assessment.Tool.Xilvr.Shared.Enum;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests;

/// <summary>
/// Represents a query to retrieve field options with filtering conditions.
/// </summary>
public class GetFieldOptionsQuery : IQuery<ApiResponse<FieldOptionsResponseDto>>
{
    /// <summary>
    /// Gets or sets the field for filtering results
    /// </summary>    
    public string? Field { get; set; } = default!;

    /// <summary>
    /// Gets or sets the specific search keyword for filtering results.
    /// </summary>
    public string SearchKeyword { get; set; } = default!;

    /// <summary>
    /// Specifies the offset
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// Specifies the limit
    /// </summary>
    public int Limit { get; set; }
}

/// <summary>
/// Defines the query handler for GetFieldOptionsQuery
/// </summary>
public class GetFieldOptionsQueryHandler : IQueryHandler<GetFieldOptionsQuery, ApiResponse<FieldOptionsResponseDto>>
{
    /// <summary>
    /// The application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// Initializes a new instance of the <see cref="GetFieldOptionsQueryHandler"/> class.
    /// </summary>
    /// <param name="elasticClient">The elastic client.</param>
    /// <param name="logger">The logger.</param>
    public GetFieldOptionsQueryHandler(IApplicationDbContext applicationDbContext)
    {
        Ensure.IsNotNull(applicationDbContext, nameof(applicationDbContext));
        _dbContext = applicationDbContext;
    }


    /// <summary>
    /// Handles the GetFieldOptionsQuery, processing the request and returning ApiResponse<FieldOptionsDto>.
    /// </summary>
    /// <param name="request">The GetFieldOptionsQuery instance representing the request.</param>
    /// <param name="cancellationToken">The cancellation token for handling asynchronous operations.</param>
    /// <returns>Returns ApiResponse<FieldOptionsResponseDto> as a result of processing the query.</returns>
    public async Task<ApiResponse<FieldOptionsResponseDto>> Handle(GetFieldOptionsQuery request, CancellationToken cancellationToken)
    {
        #region LLD
        /*
            1. Create GetFieldOptionsQuery for the POST request.
            2. Develop a handler to process the request and a validator to check the request body and fields.
            3. Validate the request body structure and ensure the presence of required fields and proper formatting.
            4. Create an ENUM for the application fields.
            5. Create a FieldOptionsResponseDto to structure the response.
            6. Implement logic to get all candidate list.
            7. Use a switch case to handle each field option and SearchKeyword in the query.
            8. Implement logic for limit and offset in the query.
            9. Fetch relevant field option values from the database.
            10. Map the retrieved data into FieldOptionsResponseDto and return.
         */
        #endregion

        var searchKeyword = request.SearchKeyword != null ? request.SearchKeyword.ToLower() : string.Empty;

        var fieldName = EnumExtensions.GetEnumValueFromDescription<FilterFields>(request.Field);
        IQueryable<FieldOptionsDto> fieldOptionsQuery = null;
        var fieldOptionCount = 0;

        switch (fieldName)
        {
            case FilterFields.Batches:
                var batchesQuery = _dbContext.Batches
                           .Where(x => string.IsNullOrEmpty(request.SearchKeyword) ||
                           x.Name.ToLower().Contains(request.SearchKeyword.ToLower()))
                           .AsNoTracking();
                fieldOptionsQuery = batchesQuery
                .Select(x => new FieldOptionsDto
                {
                    Id = x.Name,
                    Value = x.Name
                })
                .Distinct()
                .OrderBy(x => x.Value);
                fieldOptionCount = batchesQuery.Select(o => o.Name).Distinct().Count();
                break;

            case FilterFields.Assessments:
                var assessmentsQuery = _dbContext.Assessments
                      .Where(r => (string.IsNullOrEmpty(request.SearchKeyword) ||
                       r.Name.ToLower().Contains(request.SearchKeyword.ToLower())
                       ))
                       .AsNoTracking();
                fieldOptionsQuery = assessmentsQuery
                .Select(r => new FieldOptionsDto
                {
                    Id = r.Name,
                    Value = r.Name
                })
                .Distinct()
                .OrderBy(x => x.Value);
                fieldOptionCount = assessmentsQuery.Select(r => r.Name).Distinct().Count();
                break;
        }
        if (fieldOptionCount == 0)
        {
            return new ApiResponse<FieldOptionsResponseDto>(
                data: new FieldOptionsResponseDto
                {
                    Total = fieldOptionCount,
                    FieldOptions = new List<FieldOptionsDto>()
                },
                message: Constants.NO_DATA);
        }
        var fieldOptions = await fieldOptionsQuery
                        .AsNoTracking()
                        .Skip(request.Offset)
                        .Take(request.Limit)
                        .ToListAsync(cancellationToken);
        var response = new FieldOptionsResponseDto
        {
            FieldOptions = fieldOptions.OrderBy(x => x.Value).ToList(),
            Total = fieldOptionCount
        };
        return new(response, Constants.SUCCESS_MSG);
    }
}
