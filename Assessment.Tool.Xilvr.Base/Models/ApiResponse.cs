namespace Assessment.Tool.Xilvr.Base.Models;

//
// Summary:
//     The Api response base class.
//
// Type parameters:
//   TData:
public class ApiResponse<TData>
{
    //
    // Summary:
    //     Single valued data type value..
    public TData Data { get; set; }

    //
    // Summary:
    //     Specifies the message
    public string Message { get; set; }

    /// <summary>
    /// Optional meta object for additional information (e.g. base64, filename, pagination)
    /// </summary>
    public object? Meta { get; set; }

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Application.Models.ApiResponse`1
    //     class.
    //
    // Parameters:
    //   data:
    //     Constructor initialisation.
    //
    //   message:
    public ApiResponse(TData data, string message)
    {
        Data = data;
        Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the ApiResponse class with meta data.
    /// </summary>
    public ApiResponse(TData data, string message, object meta)
    {
        Data = data;
        Message = message;
        Meta = meta;
    }
}