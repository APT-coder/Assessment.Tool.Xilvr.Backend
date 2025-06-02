namespace Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessments;

/// <summary>
/// Defines the <see cref="ScheduledAssessmentDto" />.
/// </summary>
public class ScheduledAssessmentDto
{
    /// <summary>
    /// Gets or sets the batch id.
    /// </summary>
    public int? BatchId { get; set; }

    /// <summary>
    /// Gets or sets the assessment id.
    /// </summary>
    public int? AssessmentId { get; set; }

    /// <summary>
    /// Gets or sets the assessment duration.
    /// </summary>
    public TimeSpan? AssessmentDuration { get; set; }

    /// <summary>
    /// Gets or sets the assessment start date and time.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the assessment end date and time.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the randomize flag.
    /// </summary>
    public bool? CanRandomizeQuestion { get; set; }

    /// <summary>
    /// Gets or sets the display result.
    /// </summary>
    public bool? CanDisplayResult { get; set; }

    /// <summary>
    /// Gets or sets the submit before end flag.
    /// </summary>
    public bool? CanSubmitBeforeEnd { get; set; }

    /// <summary>
    /// Gets or sets the assessment link.
    /// </summary>
    public string? Link { get; set; } = default!;
}
