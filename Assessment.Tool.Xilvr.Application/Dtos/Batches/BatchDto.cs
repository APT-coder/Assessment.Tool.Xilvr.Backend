namespace Assessment.Tool.Xilvr.Application.Dtos.Batches;

/// <summary>
/// Defines batch dto
/// </summary>
/// <seealso cref="BatchDto" />
public class BatchDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the batch.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the batch.
    /// </summary>
    public string Name { get; set; } = default!;
}
