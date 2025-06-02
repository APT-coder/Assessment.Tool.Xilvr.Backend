using Assessment.Tool.Xilvr.Domain.Entities;

namespace Assessment.Tool.Xilvr.Application.Contracts;

/// <summary>
/// Interface for  scheduled assessment service.
/// </summary>
public interface IScheduledAssessmentService
{
    /// <summary>
    /// Returns scheduled assessment answer with score and isCorrect calculated.
    /// </summary>
    public ScheduledAssessmentAnswer CalculateScoreForAnswer(ScheduledAssessmentAnswer answer);
}
