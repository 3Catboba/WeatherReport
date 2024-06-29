using Auth0.OidcClient;
using MongoDB.Driver;
using LearningSystem.Models;

namespace LearningSystem
{
    public partial class App : Application
    {
        private readonly Auth0Client _auth0Client;
        private readonly MongoDBHelper _mongoDBHelper;

        public App(Auth0Client auth0Client, IMongoDatabase database)
        {
            InitializeComponent();
            _auth0Client = auth0Client;
            _mongoDBHelper = new MongoDBHelper(database);

            MainPage = new NavigationPage(new LoginPage(_auth0Client, _mongoDBHelper));
        }
    }
}
