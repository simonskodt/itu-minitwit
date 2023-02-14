namespace MiniTwit.Server.Service;

public class ServiceResponse
{
    public Response response { get; init; }
}

public class ServiceResponse<T> : ServiceResponse
{
    public T? Model { get; init; }
}