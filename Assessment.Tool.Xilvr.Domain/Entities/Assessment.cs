using Assessment.Tool.Xilvr.Base.Entities;

namespace Assessment.Tool.Xilvr.Domain.Entities;

/// <summary>
/// Defines the <see cref="Assessment" />.
/// </summary>
public class Assessment : AuditFields
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
    /// Gets or sets the collection of questions
    /// </summary>
    public ICollection<Question> Questions { get; set; } = default!;

    /// <summary>
    /// Gets or sets the collection of scheduled assessments
    /// </summary>
    public ICollection<ScheduledAssessment> ScheduledAssessments { get; set; } = default!;
}
