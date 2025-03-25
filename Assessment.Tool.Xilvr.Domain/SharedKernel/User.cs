using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.Entities;
using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;

namespace Assessment.Tool.Xilvr.Domain.SharedKernel;

/// <summary>
/// Defines the <see cref="User" />.
/// </summary>
public class User : AuditFields
{
    /// <summary>
    /// Initialize empty constructor
    /// </summary>
    private User()
    {
    }

    /// <summary>
    /// Constructor for user
    /// </summary>
    private User(
        string profileImageUrl,
        string firstName,
        string lastName,
        string phones,
        Email email,
        Guid uuid,
        DateTime createdDateTime,
        string createdby,
        string password,
        UserStatus userStatus)
    {
        ProfileImageUrl = profileImageUrl;
        Phone = phones;
        FirstName = firstName;
        LastName = lastName;
        UuId = uuid;
        Email = email;
        CreatedBy = createdby;
        CreatedDateTime = createdDateTime;
        Password = password;
        LasttPasswordReset = createdDateTime;
        UserStatusId = userStatus.Id;
    }

    /// <summary>
    /// Specifies user id
    /// </summary>
    private long _userId;

    /// <summary>
    /// Gets or Sets the Id of the entity with a long data type.
    /// </summary>
    public virtual long Id
    {
        get
        {
            return _userId;
        }
        private set
        {
            _userId = value;
        }
    }

    /// <summary>
    /// Specifies user uuid
    /// </summary>
    public Guid UuId { get; private set; }

    /// <summary>
    /// Specifies profile image url
    /// </summary>
    public string? ProfileImageUrl { get; set; } = default!;

    /// <summary>
    /// Specifies user status
    /// </summary>
    public short UserStatusId { get; set; } = default!;

    /// <summary>
    /// Specifies user email
    /// </summary>
    public Email Email { get; private set; } = default!;

    /// <summary>
    /// Specifies user first name
    /// </summary>
    public string? FirstName { get; private set; } = default!;

    /// <summary>
    /// Specifies user last name
    /// </summary>
    public string? LastName { get; private set; } = default!;

    /// <summary>
    /// Specifies user phone number
    /// </summary>
    public string? Phone { get; private set; } = default!;

    /// <summary>
    /// Specifies user password
    /// </summary>
    public string? Password { get; private set; } = default!;

    /// <summary>
    /// Specifies user password reset date
    /// </summary>
    public DateTime LasttPasswordReset { get; private set; } = default!;

    /// <summary>
    /// Specifies user status
    /// </summary>
    public UserStatus UserStatus { get; private set; } = default!;

    /// <summary>
    /// Specifies the Employees associated with the user
    /// </summary>
    public ICollection<Employee> Employees { get; private set; } = default!;

    /// <summary>
    /// Specifies the user roles associated with the user
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = default!;

    /// <summary>
    /// Create a new user
    /// </summary>
    /// /// <returns></returns>
    public static User CreateUser(
        string profileImageUrl,
        string firstName,
        string lastName,
        string phone,
        Email email,
        string createdBy,
        string password,
        UserStatus userStatus)
    {
        return new User(profileImageUrl,
            firstName,
            lastName,
            phone,
            email,
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            password,
            userStatus);
    }

    /// <summary>
    /// Set user status
    /// </summary>
    /// <param name="userStatus"></param>
    public void SetUserStatus(UserStatus userStatus) => UserStatus = userStatus;

    /// <summary>
    /// Activates the user
    /// </summary>
    public void Activate() => UserStatusId = (short)UserStatusValues.Active;

    /// Determines whether [is active user].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is active user]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsActiveUser()
    {
        return UserStatusId == (short)UserStatusValues.Active;
    }

    /// <summary>
    /// Update user details
    /// </summary>
    public void UpdateUser(string firstName, string lastName, UserStatus userStatus, string? phone, string? profileImageUrl)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new Exception((ExceptionCode.BadRequest.ToString() + ": First Name Required"));
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new Exception((ExceptionCode.BadRequest.ToString() + ": Last Name Required"));
        }
        if (userStatus.Equals(null))
        {
            throw new Exception((ExceptionCode.BadRequest.ToString() + ": User Status Required"));
        }

        FirstName = firstName;
        LastName = lastName;
        UserStatusId = userStatus.Id;
        ProfileImageUrl = profileImageUrl;
        UpdatedDateTime = DateTime.UtcNow;
        UpdatedBy = firstName;
        Phone = phone;
    }

    /// <summary>
    /// Update user password
    /// </summary>
    public void UpdatePassword(string password, UserStatus userStatus)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new Exception((ExceptionCode.BadRequest.ToString() + ": Password Required"));
        }

        UserStatusId = userStatus.Id;
        LasttPasswordReset = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }
}
