using Assessment.Tool.Xilvr.Domain.Entities;
namespace Assessment.Tool.Xilvr.Application.Contracts;

/// <summary>
/// Interface for assessment service.
/// </summary>
public interface IAssessmentService
{
    /// <summary>
    /// Returns hash for a question.
    /// </summary>
    public string ComputeQuestionHash(Question question);

    /// <summary>
    /// Save questions to database.
    /// </summary>
    public Task<List<Question>> SaveQuestions(List<Question> questionList, CancellationToken cancellationToken);
}
