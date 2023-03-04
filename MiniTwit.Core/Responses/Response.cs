using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core.Error;

namespace MiniTwit.Core.Responses;

public class Response
{
    public HTTPResponse HTTPResponse { get; }
    public APIError? Error { get; }

    public Response(HTTPResponse httpResponse, ErrorType? errorType = null)
    {
        HTTPResponse = httpResponse;
        Error = errorType == null ? null : errorType.ToAPIError(HTTPResponse);
    }

    public ActionResult ToActionResult() => ConvertToActionResult();

    protected ActionResult ConvertToActionResult(object? model = null, string location = "") => HTTPResponse switch
    {
        HTTPResponse.Ok => new OkObjectResult(model),
        HTTPResponse.Created => new CreatedResult(location, model),
        HTTPResponse.NoContent => new NoContentResult(),
        HTTPResponse.BadRequest => new BadRequestObjectResult(Error),
        HTTPResponse.NotFound => new NotFoundObjectResult(Error),
        HTTPResponse.Conflict => new ConflictObjectResult(Error),
        _ => throw new NotSupportedException($"{HTTPResponse} not supported!")
    };
}

public class Response<T> : Response
{
    public T? Model { get; }

    public Response(HTTPResponse httpResponse, T? model, ErrorType? errorType = null) : base(httpResponse, errorType)
    {
        Model = model;
    }

    public ActionResult<T> ToActionResult(string location) => base.ConvertToActionResult(Model, location);
    public new ActionResult<T> ToActionResult() => base.ConvertToActionResult(Model);
}
