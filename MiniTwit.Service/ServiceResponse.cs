using MiniTwit.Core;

namespace MiniTwit.Service;

public class ServiceResponse
{
    public HTTPResponse HTTPResponse { get; init; }
}

public class ServiceResponse<T> : ServiceResponse
{
    public T? Model { get; init; }
}