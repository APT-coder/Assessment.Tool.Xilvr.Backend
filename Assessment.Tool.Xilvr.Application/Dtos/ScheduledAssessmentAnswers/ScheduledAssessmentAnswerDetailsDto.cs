using Assessment.Tool.Xilvr.Application.Dtos.Assessments;

namespace Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentAnswers;

/// <summary>
/// Defines the <see cref="ScheduledAssessmentAnswerDetailsDto" />.
/// </summary>
public class ScheduledAssessmentAnswerDetailsDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the scheduled assessment id.
    /// </summary>
    public int ScheduledAssessmentId { get; set; }

    /// <summary>
    /// Gets or sets the employee id.
    /// </summary>
    public long EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the employee name.
    /// </summary>
    public string EmployeeName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the list of question and answers.
    /// </summary>
    public List<QuestionAnswerDto> Answers { get; set; } = default!;
}
