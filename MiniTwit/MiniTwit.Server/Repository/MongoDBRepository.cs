using MongoDB.Driver;
using MongoDB.Bson;

namespace MiniTwit.Server.Repository;

public class MongoDBRepository : IMongoDBRepository {


    MongoClient dbClient;
    IMongoDatabase database;
    IMongoCollection<User> userCollection;
    IMongoCollection<Follower> followerCollection;
    IMongoCollection<BsonDocument> messageCollection;



    public MongoDBRepository(){
        dbClient = new MongoClient("mongodb://localhost:27017");
        database = dbClient.GetDatabase ("minitwit");
        userCollection = database.GetCollection<User>("user");
        followerCollection = database.GetCollection<Follower> ("follower");
        messageCollection = database.GetCollection<BsonDocument>("message");
    }

    public void InsertUser(){
        var user = new User { _id = ObjectId.GenerateNewId(), Username = "Victor", Email = "vibr@itu.dk", PwHash = "Victor123"};
        userCollection.InsertOne(user);
    }

    public User? GetUserByUserName(string userName){
        var filter = Builders<User>.Filter.Eq("username", userName);
        var user = userCollection.Find(filter).First();
        if (user != null){
            return user;
        }
        return null;
    }

}