using LearningSystem.Models;
using Auth0.OidcClient;

namespace LearningSystem;

public partial class SignupPage : ContentPage
{
    private readonly MongoDBHelper _mongoDBHelper;
    private readonly Auth0Client _auth0Client;

    public SignupPage(Auth0Client auth0Client, MongoDBHelper mongoDBHelper)
    {
        InitializeComponent();
        _mongoDBHelper = mongoDBHelper;
        _auth0Client = auth0Client;
    }

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match", "OK");
            return;
        }

        var newUser = new UserRecord
        {
            Username = username,
            Email = email,
            Password = password
        };

        try
        {
            await _mongoDBHelper.SaveUserAsync(newUser);
            await DisplayAlert("Success", "Account created successfully", "OK");
            await Navigation.PushModalAsync(new LoginPage(_auth0Client, _mongoDBHelper));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoginPage(_auth0Client, _mongoDBHelper));
    }
}
