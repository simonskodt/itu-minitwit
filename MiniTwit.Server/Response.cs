using Microsoft.AspNetCore.Mvc;

namespace MiniTwit.Server;

public enum Response
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
    public static IActionResult ToActionResult(this Response response) => response switch
    {
        Response.Success => new OkResult(),
        Response.Created => new CreatedResult("", null),
        Response.NoContent => new NoContentResult(),
        Response.BadRequest => new BadRequestResult(),
        Response.NotFound => new NotFoundResult(),
        Response.Conflict => new ConflictResult(),
        _ => throw new NotSupportedException($"{response} not supported!")
    };
}
