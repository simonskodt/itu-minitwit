namespace MiniTwit.Core;

public enum HTTPResponse
{
    Success,        // 200, Ok
    Created,        // 201
    NoContent,      // 204
    BadRequest,     // 400
    NotFound,       // 404
    Conflict        // 409
}
