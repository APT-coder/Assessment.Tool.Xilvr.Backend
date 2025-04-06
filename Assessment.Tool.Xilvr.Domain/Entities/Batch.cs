using Assessment.Tool.Xilvr.Base.Entities;

namespace Assessment.Tool.Xilvr.Domain.Entities;

/// <summary>
/// Defines the <see cref="Batch" />.
/// </summary>
public class Batch : AuditFields
{
    /// <summary>
    /// Gets or sets the unique identifier of the role.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the active status.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the collection of scheduled assessments
    /// </summary>
    public ICollection<ScheduledAssessment> ScheduledAssessments { get; set; } = new List<ScheduledAssessment>();
}
