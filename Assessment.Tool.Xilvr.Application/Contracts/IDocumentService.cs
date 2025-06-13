using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentScores;
using Assessment.Tool.Xilvr.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Assessment.Tool.Xilvr.Application.Contracts;

/// <summary>
/// Interface for document service.
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Returns excel as base64 object.
    /// </summary>
    public Task<Object> GenerateAssessmentReportAsExcel(List<ScheduledAssessmentScoreDetailsDto> scheduledAssessmentScores,
        int scheduledAssessmentId);

    /// <summary>
    /// Returns parsed questions from file.
    /// </summary>
    public Task<List<Question>> ParseQuestionsFromFile(IFormFile file);
}
