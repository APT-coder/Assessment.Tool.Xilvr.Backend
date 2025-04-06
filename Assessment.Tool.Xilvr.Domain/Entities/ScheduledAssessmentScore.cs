using Assessment.Tool.Xilvr.Base.Entities;
using Assessment.Tool.Xilvr.Domain.Aggregates;

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

    /// <summary>
    /// Gets or sets the scheduled assessment.
    /// </summary>
    public ScheduledAssessment ScheduledAssessment { get; set; } = default!;

    /// <summary>
    /// Gets or sets the employee.
    /// </summary>
    public Employee Employee { get; set; } = default!;
}
