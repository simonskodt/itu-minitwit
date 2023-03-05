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

    public async Task<DBResult<Latest>> GetAsync(CancellationToken ct = default)
    {
        var latest = await _context.Latests.Find(_ => true).FirstAsync(ct);

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
            var update = Builders<Latest>.Update.Set("LatestVal", latestVal);
            latest = _context.Latests.FindOneAndUpdate(_ => true, update);
        }

        return new DBResult<Latest>
        {
            Model = latest,
            ErrorType = null
        };
    }

    public async Task<DBResult<Latest>> UpdateAsync(int latestVal)
    {
        var latest = await _context.Latests.Find(_ => true).FirstOrDefaultAsync();

        if (latest != null)
        {
            var update = Builders<Latest>.Update.Set("LatestVal", latestVal);
            latest = await _context.Latests.FindOneAndUpdateAsync(_ => true, update);
        }

        return new DBResult<Latest>
        {
            Model = latest,
            ErrorType = null
        };
    }
}
