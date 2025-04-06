using Assessment.Tool.Xilvr.Base.Entities;
using Assessment.Tool.Xilvr.Shared.Enum;

namespace Assessment.Tool.Xilvr.Domain.Entities;

/// <summary>
/// Defines the <see cref="ScheduledAssessment" />.
/// </summary>
public class ScheduledAssessment : AuditFields
{
    /// <summary>
    /// Gets or sets the unique identifier of the assessment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the batch id.
    /// </summary>
    public int BatchId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the assessment id.
    /// </summary>
    public int AssessmentId { get; set; } = default;

    /// <summary>
    /// Gets or sets the batch.
    /// </summary>
    public Batch Batch { get; set; } = default!;

    /// <summary>
    /// Gets or sets the assessment.
    /// </summary>
    public Assessment Assessment { get; set; } = default!;

    /// <summary>
    /// Gets or sets the assessment duration.
    /// </summary>
    public TimeSpan AssessmentDuration { get; set; }

    /// <summary>
    /// Gets or sets the assessment start date and time.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the assessment end date and time.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the assessment status.
    /// </summary>
    public AssessmentStatus AssessmentStatus { get; set; } = default!;

    /// <summary>
    /// Gets or sets the can randomize questions flag.
    /// </summary>
    public bool CanRandomizeQuestion { get; set; } = default!;

    /// <summary>
    /// Gets or sets the can display result flag.
    /// </summary>
    public bool CanDisplayResult { get; set; } = default!;

    /// <summary>
    /// Gets or sets the can submit before end flag.
    /// </summary>
    public bool CanSubmitBeforeEnd { get; set; } = default!;

    /// <summary>
    /// Gets or sets the assessment link.
    /// </summary>
    public string? Link { get; set; }
}
