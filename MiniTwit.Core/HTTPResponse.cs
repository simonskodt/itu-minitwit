using Microsoft.AspNetCore.Mvc;

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

public static class ResponseExtension
{
    public static IActionResult ToActionResult(this HTTPResponse response) => response switch
    {
        HTTPResponse.Success => new OkResult(),
        HTTPResponse.Created => new CreatedResult("", null),
        HTTPResponse.NoContent => new NoContentResult(),
        HTTPResponse.BadRequest => new BadRequestResult(),
        HTTPResponse.NotFound => new NotFoundResult(),
        HTTPResponse.Conflict => new ConflictResult(),
        _ => throw new NotSupportedException($"{response} not supported!")
    };
}
