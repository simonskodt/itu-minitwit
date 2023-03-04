using Mapster;
using MiniTwit.Core.Error;

namespace MiniTwit.Core.Responses;

public class DBResult
{
    public ErrorType? ErrorType { get; init; }
}

public class DBResult<T> : DBResult
{
    public T? Model { get; init; }
    
    public TTarget ConvertModelTo<TTarget>()
    {
        if (Model == null)
        {
            throw new InvalidOperationException($"Cannot convert {null} to {typeof(TTarget)}");
        }

        return Model.Adapt<TTarget>(); 
    }
}

