using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Assessment.Tool.Xilvr.Application.Services;

/// <summary>
/// Assessment Service
/// </summary>
public class AssessmentService : IAssessmentService
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Token service
    /// </summary>
    private readonly ITokenService _tokenService;

    public AssessmentService(IApplicationDbContext dbContext, ITokenService tokenService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
    }

    /// <summary>
    /// Returns hash for a question.
    /// </summary>
    public string ComputeQuestionHash(Question question)
    {
        var normalizedOptions = question.Options.OrderBy(x => x).ToList();
        var normalizedAnswers = question.Answer.OrderBy(x => x).ToList();

        var combined = new
        {
            question.Text,
            question.QuestionType,
            Options = normalizedOptions,
            Answers = normalizedAnswers,
            question.Points
        };

        var json = JsonSerializer.Serialize(combined);
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        return Convert.ToHexString(hashBytes);
    }

    /// <summary>
    /// Save questions to database.
    /// </summary>s
    public async Task<List<Question>> SaveQuestions(List<Question> questionList, CancellationToken cancellationToken)
    {
        foreach (var q in questionList)
        {
            q.ContentHash = ComputeQuestionHash(q);
        }

        var incomingHashes = questionList.Select(q => q.ContentHash).ToList();

        var existingQuestions = await _dbContext.Questions
            .Where(q => incomingHashes.Contains(q.ContentHash))
            .ToListAsync(cancellationToken);

        var email = _tokenService.TryGetEmailFromToken();
        var assessmentQuestions = new List<Question>();

        foreach (var incomingQuestion in questionList)
        {
            var existing = existingQuestions
                .FirstOrDefault(q => q.ContentHash == incomingQuestion.ContentHash);

            if (existing != null)
            {
                assessmentQuestions.Add(existing);
                existing.UpdatedBy = email;
                existing.UpdatedDateTime = DateTime.UtcNow;
                _dbContext.Questions.Update(existing);
            }
            else
            {
                incomingQuestion.CreatedDateTime = DateTime.UtcNow;
                incomingQuestion.CreatedBy = email;
                assessmentQuestions.Add(incomingQuestion);
                _dbContext.Questions.Add(incomingQuestion);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return assessmentQuestions;
    }
}
