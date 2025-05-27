using Assessment.Tool.Xilvr.Shared.Enum;

namespace Assessment.Tool.Xilvr.Application.Dtos.Assessments;

/// <summary>
/// Defines questions dto
/// </summary>
/// <seealso cref="QuestionsDto" />
public class QuestionsDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the question
    /// </summary>
    public int Id { get; set; }

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
