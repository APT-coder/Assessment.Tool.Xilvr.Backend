using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Domain.Entities;

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
}
