using MiniTwit.Core.Entities;

namespace MiniTwit.Infrastructure.Data.DataCreators;

public static class MessageCreator
{
    public static Message Create(string authorId, string text, DateTime pubDate, int flagged = 0)
    {
        return new Message
        {
            AuthorId = authorId,
            Text = text,
            PubDate = pubDate,
            Flagged = flagged
        };
    }

    public static Message Create(string authorId, string text, int flagged = 0)
    {
        return new Message
        {
            AuthorId = authorId,
            Text = text,
            PubDate = DateTime.Now,
            Flagged = flagged
        };
    }
}
