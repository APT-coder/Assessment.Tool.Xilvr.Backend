using System.Runtime.Serialization;

namespace Assessment.Tool.Xilvr.Base.Shared.Exceptions;

//
// Summary:
//     Base Exception class fot Xilvr System.Exception
[Serializable]
public class XilvrException : Exception
{
    //
    // Summary:
    //     Gets or sets the exception code.
    //
    // Value:
    //     The exception code.
    public int ExceptionCode { get; set; }

    //
    // Summary:
    //     Gets or sets the unique code for the exception within the system.
    //
    // Value:
    //     The unique system code.
    public string? UniqueSystemCode { get; set; }

    //
    // Summary:
    //     Gets or sets the exception message.
    //
    // Value:
    //     The exception message.
    public string? ExceptionMessage { get; set; }

    //
    // Summary:
    //     Gets or sets the exception message parmeters.
    //
    // Value:
    //     The message values.
    public string[]? MessageValues { get; set; }

    //
    // Summary:
    //     System Generated Error code, to identify errors.
    public string ErrorCode { get; set; }

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Shared.Exceptions.XilvrException class.
    public XilvrException()
    {
    }

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Shared.Exceptions.XilvrException class.
    //
    //
    // Parameters:
    //   message:
    //     The message that describes the error.
    public XilvrException(string message)
        : base(message)
    {
    }

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Shared.Exceptions.XilvrException class.
    //
    //
    // Parameters:
    //   message:
    //     The error message that explains the reason for the exception.
    //
    //   innerException:
    //     The exception that is the cause of the current exception, or a null reference
    //     (Nothing in Visual Basic) if no inner exception is specified.
    public XilvrException(string message, Exception innerException)
        : base(message, innerException)
    {
        ExceptionMessage = message;
    }

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Shared.Exceptions.XilvrException class.
    //
    //
    // Parameters:
    //   code:
    //     The code.
    //
    //   message:
    //     The message.
    public XilvrException(int code, string message)
        : base(message)
    {
        ExceptionCode = code;
        ExceptionMessage = message;
    }

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Shared.Exceptions.XilvrException class.
    //
    //
    // Parameters:
    //   code:
    //     The code.
    //
    //   message:
    //     The message.
    //
    //   innerException:
    //     The inner exception.
    public XilvrException(int code, string message, Exception innerException)
        : base(message, innerException)
    {
        ExceptionCode = code;
        ExceptionMessage = message;
    }

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Shared.Exceptions.XilvrException class.
    //
    //
    // Parameters:
    //   code:
    //     The code.
    //
    //   message:
    //     The message.
    public XilvrException(ExceptionCode code, string message)
        : base(message)
    {
        ExceptionCode = (int)code;
        ExceptionMessage = message;
    }

    //
    // Summary:
    //     Constructs the Exception with code, messag and error code.
    //
    // Parameters:
    //   code:
    //
    //   message:
    //
    //   errorCode:
    public XilvrException(ExceptionCode code, string message, string errorCode)
        : base(message)
    {
        ExceptionCode = (int)code;
        ExceptionMessage = message;
        ErrorCode = errorCode;
    }

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Shared.Exceptions.XilvrException class.
    //
    //
    // Parameters:
    //   code:
    //     The code.
    //
    //   message:
    //     The message.
    //
    //   messageValues:
    //     The messageValues.
    public XilvrException(ExceptionCode code, string message, string[] messageValues)
        : base(message)
    {
        ExceptionCode = (int)code;
        ExceptionMessage = message;
        MessageValues = messageValues;
    }

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Shared.Exceptions.XilvrException class.
    //
    //
    // Parameters:
    //   code:
    //     The code.
    //
    //   message:
    //     The message.
    //
    //   innerException:
    //     The inner exception.
    public XilvrException(ExceptionCode code, string message, Exception innerException)
        : base(message, innerException)
    {
        ExceptionCode = (int)code;
        ExceptionMessage = message;
    }

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Shared.Exceptions.XilvrException class.
    //
    //
    // Parameters:
    //   info:
    //     The System.Runtime.Serialization.SerializationInfo that holds the serialized
    //     object data about the exception being thrown.
    //
    //   context:
    //     The System.Runtime.Serialization.StreamingContext that contains contextual information
    //     about the source or destination.
    private XilvrException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ExceptionCode = info.GetInt32("ExceptionCode");
        ExceptionMessage = info.GetString("ExceptionMessage");
    }

    //
    // Summary:
    //     When overridden in a derived class, sets the System.Runtime.Serialization.SerializationInfo
    //     with information about the exception.
    //
    // Parameters:
    //   info:
    //     The System.Runtime.Serialization.SerializationInfo that holds the serialized
    //     object data about the exception being thrown.
    //
    //   context:
    //     The System.Runtime.Serialization.StreamingContext that contains contextual information
    //     about the source or destination.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     info
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
        {
            throw new ArgumentNullException("info");
        }

        info.AddValue("ExceptionCode", ExceptionCode);
        info.AddValue("ExceptionMessage", ExceptionMessage);
        base.GetObjectData(info, context);
    }
}