using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearningSystem.Models;

namespace LearningSystem
{
    public class MongoDBHelper
    {
        private readonly IMongoDatabase _database;

        public MongoDBHelper(IMongoDatabase database)
        {
            _database = database;
        }

        private IMongoCollection<UserRecord> GetUsersCollection()
        {
            return _database.GetCollection<UserRecord>("Users");
        }

        public async Task<List<UserRecord>> GetUsersAsync()
        {
            var collection = GetUsersCollection();
            var users = await collection.Find(user => true).ToListAsync();
            return users;
        }

        public async Task<UserRecord> GetUserByEmailAsync(string email)
        {
            var collection = GetUsersCollection();
            var filter = Builders<UserRecord>.Filter.Eq("Email", email);
            var user = await collection.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task SaveUserAsync(UserRecord user)
        {
            var collection = GetUsersCollection();
            var filter = Builders<UserRecord>.Filter.Eq("Email", user.Email);
            var existingUser = await collection.Find(filter).FirstOrDefaultAsync();

            if (existingUser == null)
            {
                await collection.InsertOneAsync(user);
            }
            else
            {
                await collection.ReplaceOneAsync(filter, user);
            }
        }

        public async Task UpdateUserAsync(UserRecord user)
        {
            var collection = GetUsersCollection();
            var filter = Builders<UserRecord>.Filter.Eq("Email", user.Email);
            await collection.ReplaceOneAsync(filter, user);
        }
        public async Task DeleteUserAsync(string email)
        {
            var collection = GetUsersCollection();
            var filter = Builders<UserRecord>.Filter.Eq("Email", email);
            await collection.DeleteOneAsync(filter);
        }
    }
}
