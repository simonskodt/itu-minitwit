using MongoDB.Bson;
using MongoDB.Driver;

namespace FlagTool;

class FlagTool
{
    static string docStr = "ITU-Minitwit Tweet Flagging Tool\n\n" +
                           "Usage:\n" +
                           "  ./FlagTool <tweet_id>...\n" +
                           "  ./FlagTool -i\n" +
                           "  ./FlagTool -h\n" +
                           "Options:\n" +
                           "-h            Show this screen.\n" +
                           "-i            Dump all tweets and authors to STDOUT.\n";

    static void DumpMessages(IMongoCollection<BsonDocument> messages)
    {
        var msgs = messages.Find(_ => true).ToList();
        foreach (var message in msgs)
        {
            PrintMessage(message);
        }
    }

    static void FlagMessage(IMongoCollection<BsonDocument> messages, string id)
    {
        var update = Builders<BsonDocument>.Update.Set("Flagged", 1);
        var message = messages.FindOneAndUpdate(m => m["_id"].ToString() == id, update);

        if (message == null)
        {
            throw new NullReferenceException($"Flag Error: id {id} does not exist!");
        }

        PrintMessage(message);
    }

    static void PrintMessage(BsonDocument message)
    {
        Console.WriteLine($"{message["_id"]}, {message["AuthorId"]}, {message["Text"]}, {message["PubDate"]}, {message["Flagged"]}");
    }

    static void PrintHelp()
    {
        Console.WriteLine($"{docStr}");
    }

    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "-h")
        {
            PrintHelp();
            return;
        }

        try
        {
            var password = File.ReadAllText(".local/db_password.txt");
            // var mongoClient = new MongoClient($"mongodb://radiator:{password}@164.92.167.188:27018");
            var mongoClient = new MongoClient($"mongodb://radiator:{password}@localhost:27018");
            var mongoDatabase = mongoClient.GetDatabase("MiniTwit");
            var messages = mongoDatabase.GetCollection<BsonDocument>("Tweets");

            if (args[0] == "-i")
            {
                DumpMessages(messages);
            }
            else if (args.Length >= 1)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    FlagMessage(messages, args[i]);
                    Console.WriteLine($"Flagged entry: {args[i]}");
                }
            }
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"Database password file not found: {e.Message}");
        }
        catch (MongoConfigurationException e)
        {
            Console.WriteLine($"Wrong connection string format: {e.Message}");
        }
        catch (System.TimeoutException e)
        {
            Console.WriteLine($"Could not connect to database: {e.Message}");
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
