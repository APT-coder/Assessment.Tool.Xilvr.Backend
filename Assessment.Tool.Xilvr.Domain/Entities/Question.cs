using Assessment.Tool.Xilvr.Base.Entities;
using Assessment.Tool.Xilvr.Shared.Enum;

namespace Assessment.Tool.Xilvr.Domain.Entities;

/// <summary>
/// Defines the <see cref="Question" />.
/// </summary>
public class Question : AuditFields
{
    /// <summary>
    /// Gets or sets the unique identifier of the question
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the assessment id.
    /// </summary>
    public int AssessmentId { get; set; } = default!;

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
}
