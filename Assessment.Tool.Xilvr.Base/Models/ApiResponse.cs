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
}