using MiniTwit.Core.Entities;

namespace MiniTwit.Infrastructure.Data.DataCreators;

public static class FollowerCreator
{
    public static Follower Create(string whoId, string whomId)
    {
        return new Follower
        {
            WhoId = whoId,
            WhomId = whomId
        };
    }
}
