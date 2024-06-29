using Auth0.OidcClient;

namespace LearningSystem
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly Auth0Client _auth0Client;
        private readonly MongoDBHelper _mongoDBHelper;

        public MainPage(Auth0Client auth0Client, MongoDBHelper mongoDBHelper)
        {
            InitializeComponent();
            _auth0Client = auth0Client;
            _mongoDBHelper = mongoDBHelper;
        }

        private async void BtnGetStarted_Clicked(object sender, EventArgs e)
        {
            // Ensure LoginPage is created with Auth0Client and MongoDBHelper
            await Navigation.PushModalAsync(new LoginPage(_auth0Client, _mongoDBHelper));
        }
    }
}
