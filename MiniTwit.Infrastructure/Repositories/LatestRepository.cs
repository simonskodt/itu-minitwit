using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure.Repositories;

public class LatestRepository : ILatestRepository
{
    private IMongoDBContext _context;

    public LatestRepository(IMongoDBContext context)
    {
        _context = context;
    }

    public Response<Latest> GetLatest()
    {
        var latest = _context.Latests.Find(_ => true).First();

        if (latest is null)
        {
            latest!.LatestVal = -1;
        }

        return new Response<Latest>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = latest
        };
    }

    public Response<Latest> Update(int latestVal)
    {
        var latest = _context.Latests.Find(_ => true).FirstOrDefault();

        if (latest is not null)
        {
            _context.Latests.DeleteOne(_ => true);
        }

        latest = new Latest
        {
            LatestVal = latestVal
        };

        _context.Latests.InsertOne(latest);

        return new Response<Latest>
        {
            HTTPResponse = HTTPResponse.Created,
            Model = latest
        };
    }
}