using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using MiniTwit.Core.Responses;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure.Repositories;

public class LatestRepository : ILatestRepository
{
    private IMongoDBContext _context;

    public LatestRepository(IMongoDBContext context)
    {
        _context = context;
    }

    public DBResult<Latest> Get(CancellationToken ct = default)
    {
        var latest = _context.Latests.Find(_ => true).First(ct);

        if (latest is null)
        {
            latest!.LatestVal = -1;
        }

        return new DBResult<Latest>
        {
            Model = latest,
            ErrorType = null
        };
    }

    public DBResult<Latest> Update(int latestVal)
    {
        var latest = _context.Latests.Find(_ => true).FirstOrDefault();

        if (latest != null)
        {
            var update = Builders<Latest>.Update.Set("latestVal", latest);
            latest = _context.Latests.FindOneAndUpdate(_ => true, update);
        }

        return new DBResult<Latest>
        {
            Model = latest,
            ErrorType = null
        };
    }
}
