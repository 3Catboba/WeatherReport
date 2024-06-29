using Auth0.OidcClient;
using LearningSystem.ViewModels;

namespace LearningSystem;

public partial class LoginPage : ContentPage
{
    public LoginPage(Auth0Client auth0Client, MongoDBHelper mongoDBHelper)
    {
        InitializeComponent();
        BindingContext = new LoginViewModel(auth0Client, mongoDBHelper);
    }
}
