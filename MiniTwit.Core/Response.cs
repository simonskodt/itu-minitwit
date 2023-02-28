using Microsoft.AspNetCore.Mvc;

namespace MiniTwit.Core;

public class Response
{
    public HTTPResponse HTTPResponse { get; init; }
}

public class Response<T> : Response
{
    public T? Model { get; init; }
}

public static class ResponseExtension
{
    public static IActionResult ToActionResult(this Response response) => ToActionResult(response, null);
    public static IActionResult ToActionResult<T>(this Response<T> response) => ToActionResult(response, response.Model);
    public static IActionResult ToActionResult<T>(this Response<T> response, string location) => ToActionResult(response, response.Model, location);

    private static IActionResult ToActionResult(this Response response, object? model, string location = "") => response.HTTPResponse switch
    {
        HTTPResponse.Success => new OkObjectResult(model),
        HTTPResponse.Created => new CreatedResult(location, model),
        HTTPResponse.NoContent => new NoContentResult(),
        HTTPResponse.BadRequest => new BadRequestResult(),
        HTTPResponse.NotFound => new NotFoundResult(),
        HTTPResponse.Conflict => new ConflictResult(),
        _ => throw new NotSupportedException($"{response} not supported!")
    };
}
