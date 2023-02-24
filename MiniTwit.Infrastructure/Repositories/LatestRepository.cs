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

        return new Response<Latest>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = latest
        };
    }


/*     public void Update()
    {
        //var latest = _context.Latests.Find(_ => true).;




    } */
}