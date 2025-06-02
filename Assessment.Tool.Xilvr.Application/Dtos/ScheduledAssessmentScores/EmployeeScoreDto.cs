namespace Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentScores;

/// <summary>
/// Defines the <see cref="EmployeeScoreDto" />.
/// </summary>
public class EmployeeScoreDto
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
}
