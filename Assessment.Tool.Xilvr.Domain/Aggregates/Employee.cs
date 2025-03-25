using Assessment.Tool.Xilvr.Base.Entities;
using Assessment.Tool.Xilvr.Domain.SharedKernel;

namespace Assessment.Tool.Xilvr.Domain.Aggregates;

/// <summary>
/// Defines the <see cref="Employee" />.
/// </summary>
public class Employee : AuditFields
{
    /// <summary>
    /// Initialize empty constructor
    /// </summary>
    private Employee()
    {
    }

    /// <summary>
    /// Constructor for employee
    /// </summary>
    public Employee(User user, string? designation, bool isActive, List<string> batchIds,
                        string createdBy, DateTime createdOn)
    {
        User = user;
        Designation = designation;
        IsActive = isActive;
        BatchIds = batchIds;
        CreatedBy = createdBy;
        CreatedDateTime = createdOn;
    }

    /// <summary>
    /// Specifies the employee id
    /// </summary>
    private long _employeeId;

    /// <summary>
    /// Gets or Sets the Id of the entity with a long data type.
    /// </summary>
    public virtual long Id
    {
        get
        {
            return _employeeId;
        }
        private set
        {
            _employeeId = value;
        }
    }

    /// <summary>
    /// Specifies user id
    /// </summary>
    public long UserId { get; private set; }

    /// <summary>
    /// Specifies list of batch Ids
    /// </summary>
    public List<string> BatchIds { get; private set; } = new List<string>();

    /// <summary>
    /// Specifies the user
    /// </summary>
    public User User { get; private set; } = default!;

    /// <summary>
    /// Specifies employee designation
    /// </summary>
    public string? Designation { get; set; }

    /// <summary>
    /// Specifies the active status
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Create employee
    /// </summary>
    public static Employee Create(string profileImageUrl, string firstName, string lastName, string emailId, List<string> batchIds,
                                    string phoneNumber, string createdBy, string password, string designation, bool isActive)
    {
        var email = new Email(emailId);
        var userStatus = UserStatus.SetFrom(UserStatusValues.Pending);
        var user = User.CreateUser(profileImageUrl, firstName, lastName, phoneNumber, email,
                                    createdBy, password, userStatus);

        var employee = new Employee(user, designation, isActive, batchIds, createdBy, DateTime.UtcNow);
        return employee;
    }

    /// <summary>
    /// Update employee details
    /// </summary>
    public void Update(string firstName, string lastName, UserStatus userStatus, string phone,
                        string profileImageUrl, string designation, List<string> batchIds)
    {
        User.UpdateUser(firstName, lastName, userStatus, phone, profileImageUrl);

        Designation = designation;
        BatchIds = batchIds;
        UpdatedDateTime = DateTime.UtcNow;
        UpdatedBy = firstName;
    }

    public void ActivateUser()
    {
        this.User.Activate();
    }
}
