namespace Assessment.Tool.Xilvr.Base;

//
// Summary:
//     Defines the exception codes
public enum ExceptionCode
{
    //
    // Summary:
    //     BadRequest Http 400
    BadRequest = 400,
    //
    // Summary:
    //     ForBidden Http 403
    UnauthorizedAccess = 403,
    //
    // Summary:
    //     Not Found Http 404
    NotFound = 404,
    //
    // Summary:
    //     UnAuthorized Http 401
    TokenExpired = 401,
    //
    // Summary:
    //     Precondition Failed Http 412
    PreconditionFailed = 412,
    //
    // Summary:
    //     Un processable entity http 422
    UnprocessableEntity = 422,
    //
    // Summary:
    //     Too Many Requests Http 429
    TooManyRequests = 429,
    //
    // Summary:
    //     Internal Server Error Http 500
    InternalServerError = 500,
    //
    // Summary:
    //     Service Unavailable http 503
    ServiceUnavailable = 503
}
