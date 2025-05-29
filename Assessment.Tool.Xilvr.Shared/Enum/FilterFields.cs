using System.ComponentModel;

namespace Assessment.Tool.Xilvr.Shared.Enum;

/// <summary>
/// Defines filter filelds
/// </summary>
public enum FilterFields
{
    /// <summary>
    /// Specifies the batches
    /// </summary>
    [Description("Batches")]
    Batches,

    /// <summary>
    /// Specifies the assessments
    /// </summary>
    [Description("assessments")]
    Assessments,

    /// <summary>
    /// Specifies the assessment status
    /// </summary>
    [Description("assessmentStatus")]
    AssessmentStatus,
}
