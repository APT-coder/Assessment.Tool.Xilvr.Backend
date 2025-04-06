namespace Assessment.Tool.Xilvr.Shared.Constants;

/// <summary>
/// Defines the <see cref="Constants" />.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Max length of user name
    /// </summary>
    public const int USER_NAME_LENGTH = 100;

    /// <summary>
    /// Max length of user status
    /// </summary>
    public const int USER_STATUS_LENGTH = 15;

    /// <summary>
    /// Specifies the max length of profile image   
    /// </summary>
    public const int PROFILE_IMAGE_URL_LENGTH = 100;

    /// <summary>
    /// Max length of email
    /// </summary>
    public const int EMAIL_LENGTH = 100;

    /// <summary>
    /// Max length of created by & updated by
    /// </summary>
    public const int CREATED_BY_AND_UPDATED_BY_LENGTH = 100;

    /// <summary>
    /// Password pattern check
    /// </summary>
    public const string PASSWORD_REGEX =
        "Password LIKE '%[A-Z]%' AND Password LIKE '%[0-9]%' AND Password LIKE '%[@#_]%' AND LEN(Password) >= 8";


    /// <summary>
    /// Specfies the success message
    /// </summary>
    public const string SUCCESS_MSG = "Success.";

    /// <summary>
    /// Specifies the no data message
    /// </summary>
    public const string NO_DATA = "No Data.";
}
