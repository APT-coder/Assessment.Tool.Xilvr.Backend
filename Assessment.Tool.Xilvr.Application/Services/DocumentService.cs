using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentScores;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Enum;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using Xceed.Words.NET;

namespace Assessment.Tool.Xilvr.Application.Services;

/// <summary>
/// Document Service
/// </summary>
public class DocumentService : IDocumentService
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Token service
    /// </summary>
    private readonly ITokenService _tokenService;

    public DocumentService(IApplicationDbContext dbContext, ITokenService tokenService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
    }

    public async Task<Object> GenerateAssessmentReportAsExcel(List<ScheduledAssessmentScoreDetailsDto> scheduledAssessmentScores,
        int scheduledAssessmentId)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Aswin");
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Scores");

        worksheet.Cells[1, 1].Value = "Employee Name";
        worksheet.Cells[1, 2].Value = "Email";
        worksheet.Cells[1, 3].Value = "Batch Name";
        worksheet.Cells[1, 4].Value = "Score";
        worksheet.Cells[1, 5].Value = "Is Evaluated";

        for (int i = 0; i < scheduledAssessmentScores.Count; i++)
        {
            var s = scheduledAssessmentScores[i];
            worksheet.Cells[i + 2, 1].Value = s.EmployeeName;
            worksheet.Cells[i + 2, 2].Value = s.Email;
            worksheet.Cells[i + 2, 3].Value = s.BatchName;
            worksheet.Cells[i + 2, 4].Value = s.Score;
            worksheet.Cells[i + 2, 5].Value = s.IsEvaluated ? "Yes" : "No";
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var fileBytes = await package.GetAsByteArrayAsync();
        var base64Content = Convert.ToBase64String(fileBytes);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
        var fileName = $"Assessment_{scheduledAssessmentId}_{timestamp}.xlsx";

        var meta = new
        {
            FileName = fileName,
            Base64 = base64Content
        };

        return meta;
    }

    public async Task<List<Question>> ParseQuestionsFromFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();

        return extension switch
        {
            ".docx" => ParseWordFile(file),
            ".pdf" => ParsePdfFile(file),
            _ => throw new NotSupportedException("File format not supported")
        };
    }

    private static List<Question> ParseWordFile(IFormFile file)
    {
        var questions = new List<Question>();

        using var stream = file.OpenReadStream();
        using var doc = DocX.Load(stream);

        var lines = doc.Paragraphs.Select(p => p.Text.Trim()).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

        var currentBlock = new List<string>();
        foreach (var line in lines)
        {
            if (Regex.IsMatch(line, @"^\d+\.\s"))
            {
                if (currentBlock.Count > 0)
                {
                    var question = ParseQuestionBlock(currentBlock);
                    questions.Add(question);
                    currentBlock.Clear();
                }
            }
            currentBlock.Add(line);
        }

        if (currentBlock.Count > 0)
        {
            var question = ParseQuestionBlock(currentBlock);
            questions.Add(question);
        }

        return questions;
    }

    private static List<Question> ParsePdfFile(IFormFile file)
    {
        var questions = new List<Question>();

        using var stream = file.OpenReadStream();
        using var pdf = PdfDocument.Open(stream);

        var fullText = string.Join(Environment.NewLine, pdf.GetPages().Select(p => p.Text));
        var matches = Regex.Split(fullText, @"(?=\d+\.\s)").Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        foreach (var raw in matches)
        {
            var question = new Question
            {
                Points = 1,
                Options = new List<string>(),
                Answer = new List<string>()
            };

            var text = raw.Trim();

            var answerMatch = Regex.Match(text, @"Answer\s*:\s*(.+)", RegexOptions.IgnoreCase);
            if (answerMatch.Success)
            {
                var answerStr = answerMatch.Groups[1].Value.Trim();
                question.Answer = answerStr.Split(',').Select(a => a.Trim()).ToList();
                text = Regex.Replace(text, @"Answer\s*:\s*.+", "", RegexOptions.IgnoreCase).Trim();
            }

            var optionMatches = Regex.Matches(text, @"([A-Da-d]\.\s?.+?)(?=[A-Da-d]\.|$)", RegexOptions.Singleline);
            if (optionMatches.Count > 0)
            {
                question.Options = optionMatches.Select(m => m.Value.Trim()).ToList();
                question.QuestionType = question.Answer.Count > 1 ? QuestionType.Msq : QuestionType.Mcq;
            }
            else if (text.Contains("___"))
            {
                question.QuestionType = QuestionType.FillUp;
            }
            else
            {
                question.QuestionType = QuestionType.Descriptive;
            }

            var firstOptionIndex = question.Options.Count > 0
                ? text.IndexOf(question.Options[0])
                : text.Length;

            question.Text = text.Substring(0, firstOptionIndex).Trim();

            questions.Add(question);
        }

        return questions;
    }

    private static Question ParseQuestionBlock(List<string> lines)
    {
        var question = new Question();
        var options = new List<string>();
        var answers = new List<string>();
        int points = 1;

        var sb = new System.Text.StringBuilder();

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            var pointMatch = Regex.Match(trimmedLine, @"(?i)(points|marks)[:\s]*([0-9]+)|\((\d+)\s+points\)");
            if (pointMatch.Success)
            {
                if (int.TryParse(pointMatch.Groups[2].Value, out int parsedPoints))
                    points = parsedPoints;
                else if (int.TryParse(pointMatch.Groups[3].Value, out parsedPoints))
                    points = parsedPoints;
                continue;
            }

            if (trimmedLine.StartsWith("Answer:", StringComparison.OrdinalIgnoreCase))
            {
                var answerPart = trimmedLine.Substring("Answer:".Length).Trim();
                answers.AddRange(answerPart.Split(',').Select(a => a.Trim()));
                continue;
            }

            if (Regex.IsMatch(trimmedLine, @"^[A-D]\.") || Regex.IsMatch(trimmedLine, @"^[A-D]\)"))
            {
                options.Add(trimmedLine);
            }
            else
            {
                sb.AppendLine(trimmedLine);
            }
        }

        question.Text = sb.ToString().Trim();
        question.Options = options;
        question.Answer = answers;
        question.Points = points;
        question.QuestionType = InferQuestionType(question);

        return question;
    }

    private static QuestionType InferQuestionType(Question question)
    {
        if (question.Options.Count == 0)
            return QuestionType.Descriptive;
        if (question.Answer.Count > 1)
            return QuestionType.Msq;
        if (question.Text.Contains("___") || question.Text.Contains("__"))
            return QuestionType.FillUp;
        return QuestionType.Mcq;
    }
}
