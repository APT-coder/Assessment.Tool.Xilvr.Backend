using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentScores;
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

    /// <summary>
    /// Updates total score for given list of employees.
    /// </summary>
    public Task UpdateTotalScore(int scheduledAssessmentId, List<long> employeeIds, CancellationToken cancellationToken);

    /// <summary>
    /// Gets scheduled assessment score for a given id.
    /// </summary>
    public Task<List<ScheduledAssessmentScoreDetailsDto>> GetScoreDetailsByScheduledAssessmentIdAsync(
    int scheduledAssessmentId,
    CancellationToken cancellationToken = default);
}
