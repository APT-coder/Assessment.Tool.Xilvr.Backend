using Assessment.Tool.Xilvr.Domain.SharedKernel;

namespace Assessment.Tool.Xilvr.Application.DataSeeders;

/// <summary>
/// Defines the <see cref="UserStatusSeeder" />.
/// </summary>
public static class UserStatusSeeder
{
    /// <summary>
    /// Gets user status data
    /// </summary>
    /// <returns></returns>
    public static List<UserStatus> GetData()
    {
        return new List<UserStatus>()
        {
            UserStatus.SetFrom(UserStatusValues.Pending),
            UserStatus.SetFrom(UserStatusValues.InActive),
            UserStatus.SetFrom(UserStatusValues.Active),
        };
    }
}
