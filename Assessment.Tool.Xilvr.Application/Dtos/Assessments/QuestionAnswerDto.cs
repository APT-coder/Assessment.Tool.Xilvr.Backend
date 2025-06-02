using Assessment.Tool.Xilvr.Shared.Enum;

namespace Assessment.Tool.Xilvr.Application.Dtos.Assessments;

/// <summary>
/// Defines the <see cref="QuestionAnswerDto" />.
/// </summary>
public class QuestionAnswerDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the question
    /// </summary>
    public int QuestionId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the scheduled assessment answer
    /// </summary>
    public int ScheduledAssessmentAnswerId { get; set; }

    /// <summary>
    /// Gets or sets the question text
    /// </summary>
    public string Text { get; set; } = default!;

    /// <summary>
    /// Gets or sets the question type
    /// </summary>
    public QuestionType QuestionType { get; set; } = default!;

    /// <summary>
    /// Gets or sets the question options
    /// </summary>
    public List<string> Options { get; set; } = [];

    /// <summary>
    /// Gets or sets the question answer
    /// </summary>
    public List<string> Answer { get; set; } = [];

    /// <summary>
    /// Gets or sets the question points
    /// </summary>
    public int Points { get; set; } = default!;

    /// <summary>
    /// Gets or sets the marked answer
    /// </summary>
    public string MarkedAnswer { get; set; } = default!;

    /// <summary>
    /// Gets or sets the is correct flag
    /// </summary>
    public bool IsCorrect { get; set; } = default!;

    /// <summary>
    /// Gets or sets the score for each answer.
    /// </summary>
    public int Score { get; set; } = default!;
}
