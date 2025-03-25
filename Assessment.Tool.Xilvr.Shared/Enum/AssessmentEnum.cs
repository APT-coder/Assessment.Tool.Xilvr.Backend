using System.ComponentModel;

namespace Assessment.Tool.Xilvr.Shared.Enum;

/// <summary>
/// Defines the <see cref="QuestionType" />.
/// </summary>
public enum QuestionType
{
    [Description("Mcq")]
    Mcq = 1,

    [Description("Msq")]
    Msq = 2,

    [Description("FillUp")]
    FillUp = 3,

    [Description("Descriptive")]
    Descriptive = 4
}

/// <summary>
/// Defines the <see cref="AssessmentStatus" />.
/// </summary>
public enum AssessmentStatus
{
    [Description("Upcoming")]
    Upcoming = 1,

    [Description("In Progress")]
    InProgress = 2,

    [Description("Completed")]
    Completed = 3,

    [Description("Evaluated")]
    Evaluated = 4,

    [Description("Cancelled")]
    Cancelled = 5,

    [Description("Rescheduled")]
    Rescheduled = 6,
}
