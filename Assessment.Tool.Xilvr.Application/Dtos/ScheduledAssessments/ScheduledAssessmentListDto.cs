using Assessment.Tool.Xilvr.Shared.Enum;

namespace Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessments;

/// <summary>
/// Defines the <see cref="ScheduledAssessmentListDto" />.
/// </summary>
public class ScheduledAssessmentListDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the assessment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the batch name.
    /// </summary>
    public string BatchName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the assessment name.
    /// </summary>
    public string AssessmentName { get; set; } = default!;

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
}
