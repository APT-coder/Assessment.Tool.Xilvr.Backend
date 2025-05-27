namespace Assessment.Tool.Xilvr.Application.Dtos.Assessments;

/// <summary>
/// Defines assessment details dto
/// </summary>
/// <seealso cref="AssessmentDetailsDto" />
public class AssessmentDetailsDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the assessment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the assessment.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the total marks.
    /// </summary>
    public int TotalMarks { get; set; } = default!;

    /// <summary>
    /// Gets or sets the list of questions.
    /// </summary>
    public List<QuestionsDto> Questions { get; set; } = default!;
}
