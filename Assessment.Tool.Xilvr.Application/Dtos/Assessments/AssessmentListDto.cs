namespace Assessment.Tool.Xilvr.Application.Dtos.Assessments;

/// <summary>
/// Defines assessment list dto
/// </summary>
/// <seealso cref="AssessmentListDto" />
public class AssessmentListDto
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
}
