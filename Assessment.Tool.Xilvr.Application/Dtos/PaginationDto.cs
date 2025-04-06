namespace Assessment.Tool.Xilvr.Application.Dtos;

/// <summary>
/// Define <see cref="PaginationDto">
/// </summary>
public class PaginationDto
{
    /// <summary>
    /// The number of records to skip in the result set.
    /// </summary>
    public int Skip { get; set; }

    /// <summary>
    /// The maximum number of records to return in the result set.
    /// </summary>
    public int Take { get; set; }
}
