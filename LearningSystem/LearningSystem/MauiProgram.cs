using Microsoft.Extensions.Logging;
using Auth0.OidcClient;
using MongoDB.Driver;
using MongoDB.Bson;
using System;

namespace LearningSystem
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Add Auth0 configuration
            builder.Services.AddSingleton(sp => new Auth0Client(new Auth0ClientOptions
            {
                Domain = "dev-zmbaoziy0km156lm.us.auth0.com",
                ClientId = "yMuUlxBaEjOruHfaLPIj2uSpQ57lEx2d",
                RedirectUri = "myapp://callback"
            }));

            // Standard MongoDB connection string
            const string connectionUri = "mongodb://admin:mTncb4TE2AFDEwSC@ac-ddxohoz-shard-00-00.4kt7st8.mongodb.net:27017,ac-ddxohoz-shard-00-01.4kt7st8.mongodb.net:27017,ac-ddxohoz-shard-00-02.4kt7st8.mongodb.net:27017/?replicaSet=atlas-xdrjrp-shard-0&ssl=true&authSource=admin&retryWrites=true&w=majority&appName=Cluster0";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);

            // Create a new MongoClient and connect to the server
            var mongoClient = new MongoClient(settings);
            var database = mongoClient.GetDatabase("LearningSystem"); // Ensure the database name is correct

            // Test the connection by sending a ping
            try
            {
                var result = database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MongoDB: {ex.Message}");
                throw;
            }

            builder.Services.AddSingleton(database);
            builder.Services.AddSingleton<MongoDBHelper>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
