namespace Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentAnswers;

/// <summary>
/// Defines the <see cref="ScheduledAssessmentAnswerDto" />.
/// </summary>
public class ScheduledAssessmentAnswerDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the scheduled assessment id.
    /// </summary>
    public int ScheduledAssessmentId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the employee id.
    /// </summary>
    public long EmployeeId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the question id.
    /// </summary>
    public int QuestionId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the answer
    /// </summary>
    public string Answer { get; set; } = default!;
}
