using Assessment.Tool.Xilvr.Domain.Aggregates;

namespace Assessment.Tool.Xilvr.Domain.Entities;

/// <summary>
/// Defines the <see cref="ScheduledAssessmentAnswer" />.
/// </summary>
public class ScheduledAssessmentAnswer
{
    /// <summary>
    /// Gets or sets the unique identifier of the scheduled assessment answer.
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
    /// Gets or sets the question id.
    /// </summary>
    public int QuestionId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the answer
    /// </summary>
    public string Answer { get; set; } = default!;

    /// <summary>
    /// Gets or sets the is correct flag
    /// </summary>
    public bool IsCorrect { get; set; } = default!;

    /// <summary>
    /// Gets or sets the score for each answer.
    /// </summary>
    public int Score { get; set; } = default!;

    /// <summary>
    /// Gets or sets the scheduled assessment.
    /// </summary>
    public ScheduledAssessment ScheduledAssessment { get; set; } = default!;

    /// <summary>
    /// Gets or sets the question.
    /// </summary>
    public Question Question { get; set; } = default!;

    /// <summary>
    /// Gets or sets the employee.
    /// </summary>
    public Employee Employee { get; set; } = default!;
}
