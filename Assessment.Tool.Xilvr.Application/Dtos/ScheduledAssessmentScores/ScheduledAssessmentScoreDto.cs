using Assessment.Tool.Xilvr.Application.Dtos.Employees;

namespace Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentScores;

/// <summary>
/// Defines the <see cref="ScheduledAssessmentScoreDto" />.
/// </summary>
public class ScheduledAssessmentScoreDto
{
    /// <summary>
    /// Gets or sets the list of attendees.
    /// </summary>
    public List<EmployeeScoreDto> Attendees { get; set; } = default!;

    /// <summary>
    /// Gets or sets the list of absentees.
    /// </summary>
    public List<EmployeeDto> Absentees { get; set; } = default!;

    /// <summary>
    /// Gets or sets the attendance count.
    /// </summary>
    public string Attendance { get; set; } = default!;
}
