namespace Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentAnswers;

/// <summary>
/// Defines the <see cref="AnswerScoresDto" />.
/// </summary>
public class AnswerScoresDto
{
    /// <summary>
    /// Gets or sets the employee id.
    /// </summary>
    public long EmployeeId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the question id.
    /// </summary>
    public int QuestionId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the score for each answer.
    /// </summary>
    public int Score { get; set; } = default!;
}
