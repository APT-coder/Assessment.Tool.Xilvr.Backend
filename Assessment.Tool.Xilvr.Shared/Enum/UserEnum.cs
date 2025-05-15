using System.ComponentModel;

namespace Assessment.Tool.Xilvr.Shared.Enum;

/// <summary>
/// Defines the <see cref="UserProvider" />.
/// </summary>
public enum UserProvider
{
    [Description("Local")]
    Local = 1,

    [Description("Google")]
    Google = 2,

    [Description("Microsoft")]
    Microsoft = 3,
}
