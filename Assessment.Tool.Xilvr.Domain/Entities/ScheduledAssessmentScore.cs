using Assessment.Tool.Xilvr.Base.Entities;

namespace Assessment.Tool.Xilvr.Domain.Entities;

/// <summary>
/// Defines the <see cref="ScheduledAssessmentScore" />.
/// </summary>
public class ScheduledAssessmentScore : AuditFields
{
    /// <summary>
    /// Gets or sets the unique identifier of the assessment score.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the scheduled assessment id.
    /// </summary>
    public int ScheduledAssessmentId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the employee id.
    /// </summary>
    public long EmployeeId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the trainee score for the assessment.
    /// </summary>
    public double Score { get; set; } = default!;
}
