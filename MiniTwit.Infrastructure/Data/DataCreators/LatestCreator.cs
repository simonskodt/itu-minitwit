using MiniTwit.Core.Entities;

namespace MiniTwit.Infrastructure.Data.DataCreators;

public static class LatestCreator
{
    public static Latest Create(int latest)
    {
        return new Latest
        {
            LatestVal = latest
        };
    }
}
