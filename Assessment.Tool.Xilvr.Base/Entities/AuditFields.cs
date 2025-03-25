namespace Assessment.Tool.Xilvr.Base.Entities;

/// <summary>
/// Represents the common audit fields
/// </summary>
public class AuditFields
{
    /// <summary>
    /// Gets or sets the created date time.
    /// </summary>
    public DateTime? CreatedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the created by.
    /// </summary>
    public string? CreatedBy { get; set; } = default!;

    /// <summary>
    /// Gets or sets the updated date time.
    /// </summary>
    public DateTime? UpdatedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the updated by.
    /// </summary>
    public string? UpdatedBy { get; set; } = default!;
}

