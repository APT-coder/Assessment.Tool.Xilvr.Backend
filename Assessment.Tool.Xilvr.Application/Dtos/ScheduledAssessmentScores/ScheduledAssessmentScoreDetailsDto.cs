namespace Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentScores;

/// <summary>
/// Defines the <see cref="ScheduledAssessmentScoreDetailsDto" />.
/// </summary>
public class ScheduledAssessmentScoreDetailsDto
{
    /// <summary>
    /// Gets or sets the employee id.
    /// </summary>
    public long EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the employee name.
    /// </summary>
    public string EmployeeName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the trainee score for the assessment.
    /// </summary>
    public double Score { get; set; } = default!;

    /// <summary>
    /// Gets or sets the is evaluated field.
    /// </summary>
    public bool IsEvaluated { get; set; }

    /// <summary>
    /// Gets or sets the employee email.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Gets or sets the batch name.
    /// </summary>
    public string BatchName { get; set; } = default!;
}
