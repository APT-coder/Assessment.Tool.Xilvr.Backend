using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentScores;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Services;

public class ScheduledAssessmentService : IScheduledAssessmentService
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Token service
    /// </summary>
    private readonly ITokenService _tokenService;

    public ScheduledAssessmentService(IApplicationDbContext dbContext, ITokenService tokenService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
    }

    public ScheduledAssessmentAnswer CalculateScoreForAnswer(ScheduledAssessmentAnswer answer)
    {
        if (answer.Question == null || string.IsNullOrWhiteSpace(answer.Answer))
        {
            answer.Score = 0;
            answer.IsCorrect = false;
            return answer;
        }

        var correctAnswers = answer.Question.Answer ?? new List<string>();

        switch (answer.Question.QuestionType)
        {
            case Shared.Enum.QuestionType.Mcq:
                if (correctAnswers.Contains(answer.Answer.Trim()))
                {
                    answer.Score = answer.Question.Points;
                    answer.IsCorrect = true;
                }
                else
                {
                    answer.Score = 0;
                    answer.IsCorrect = false;
                }
                break;

            case Shared.Enum.QuestionType.Msq:
                var givenAnswers = answer.Answer
                    .Split("&%", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                int correctCount = givenAnswers.Count(ans => correctAnswers.Contains(ans));
                int totalCorrect = correctAnswers.Count;

                answer.Score = (correctCount * answer.Question.Points) / totalCorrect;
                answer.IsCorrect = correctCount == totalCorrect;
                break;

            case Shared.Enum.QuestionType.FillUp:
                var correct = correctAnswers.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(correct) &&
                    string.Equals(correct.Trim(), answer.Answer.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    answer.Score = answer.Question.Points;
                    answer.IsCorrect = true;
                }
                else
                {
                    answer.Score = 0;
                    answer.IsCorrect = false;
                }
                break;

            default:
                answer.Score = 0;
                answer.IsCorrect = false;
                break;
        }

        return answer;
    }

    public async Task UpdateTotalScore(int scheduledAssessmentId, List<long> employeeIds, CancellationToken cancellationToken)
    {
        var totals = await _dbContext.ScheduledAssessmentsAnswers
            .Where(a => a.ScheduledAssessmentId == scheduledAssessmentId && employeeIds.Contains(a.EmployeeId))
            .GroupBy(a => a.EmployeeId)
            .Select(g => new
            {
                EmployeeId = g.Key,
                TotalScore = g.Sum(x => x.Score)
            })
            .ToDictionaryAsync(x => x.EmployeeId, x => x.TotalScore, cancellationToken);

        var existingScores = await _dbContext.ScheduledAssessmentsScores
            .Where(s => s.ScheduledAssessmentId == scheduledAssessmentId && employeeIds.Contains(s.EmployeeId))
            .ToListAsync(cancellationToken);

        var email = _tokenService.TryGetEmailFromToken();

        foreach (var kvp in totals)
        {
            var employeeId = kvp.Key;
            var totalScore = kvp.Value;

            var existing = existingScores.FirstOrDefault(s => s.EmployeeId == employeeId);

            if (existing != null)
            {
                existing.Score = totalScore;
                existing.UpdatedBy = email;
                existing.UpdatedDateTime = DateTime.UtcNow;
                existing.IsEvaluated = true;
                _dbContext.ScheduledAssessmentsScores.Update(existing);
            }
            else
            {
                var newScore = new ScheduledAssessmentScore
                {
                    ScheduledAssessmentId = scheduledAssessmentId,
                    EmployeeId = employeeId,
                    Score = totalScore,
                    IsEvaluated = true,
                    CreatedBy = email,
                    CreatedDateTime = DateTime.UtcNow
                };
                _dbContext.ScheduledAssessmentsScores.Add(newScore);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ScheduledAssessmentScoreDetailsDto>> GetScoreDetailsByScheduledAssessmentIdAsync(
    int scheduledAssessmentId,
    CancellationToken cancellationToken = default)
    {
        return await _dbContext.ScheduledAssessmentsScores
            .Where(s => s.ScheduledAssessmentId == scheduledAssessmentId)
            .Include(s => s.Employee)
            .Include(s => s.ScheduledAssessment)
                .ThenInclude(sa => sa.Batch)
            .Select(s => new ScheduledAssessmentScoreDetailsDto
            {
                EmployeeId = s.EmployeeId,
                EmployeeName = s.Employee.User.FirstName + s.Employee.User.LastName,
                Email = s.Employee.User.Email.EmailId,
                Score = s.Score,
                IsEvaluated = s.IsEvaluated,
                BatchName = s.ScheduledAssessment.Batch.Name
            })
            .ToListAsync(cancellationToken);
    }

}
