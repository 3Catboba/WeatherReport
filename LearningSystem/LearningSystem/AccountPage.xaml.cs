using LearningSystem.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Auth0.OidcClient; // Ensure this using directive is present

namespace LearningSystem;

public partial class AccountPage : ContentPage
{
    private readonly MongoDBHelper _mongoDBHelper;
    private readonly Auth0Client _auth0Client; // Add Auth0Client reference
    private UserRecord _currentUser;

    public AccountPage(MongoDBHelper mongoDBHelper, Auth0Client auth0Client, string userEmail)
    {
        InitializeComponent();
        _mongoDBHelper = mongoDBHelper;
        _auth0Client = auth0Client; // Initialize Auth0Client
        LoadUserData(userEmail);
    }

    private async void LoadUserData(string email)
    {
        Debug.WriteLine($"LoadUserData called with email: {email}");

        // Fetch the current user data
        _currentUser = await _mongoDBHelper.GetUserByEmailAsync(email);

        if (_currentUser != null)
        {
            Debug.WriteLine($"User found: {_currentUser.Username}");
            UsernameEntry.Text = _currentUser.Username;
            DefaultCityEntry.Text = _currentUser.DefaultCity;
            BackgroundColorPicker.SelectedItem = _currentUser.BackgroundColor ?? "White"; // Set default to White
        }
        else
        {
            Debug.WriteLine("User not found or _currentUser is null.");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_currentUser != null)
        {
            _currentUser.Username = UsernameEntry.Text;
            _currentUser.DefaultCity = DefaultCityEntry.Text;

            if (BackgroundColorPicker.SelectedItem != null)
            {
                _currentUser.BackgroundColor = BackgroundColorPicker.SelectedItem.ToString();
            }
            else
            {
                _currentUser.BackgroundColor = "White"; // Set default to White
            }

            await _mongoDBHelper.UpdateUserAsync(_currentUser);

            await DisplayAlert("Success", "Account settings updated successfully.", "OK");
            await Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Error", "User data not loaded. Please try again.", "OK");
        }
    }

    private async void OnDeleteAccountClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm", "Are you sure you want to delete your account?", "Yes", "No");

        if (confirm && _currentUser != null)
        {
            await _mongoDBHelper.DeleteUserAsync(_currentUser.Email);
            await DisplayAlert("Success", "Account deleted successfully.", "OK");

            // Clear the saved email
            Preferences.Remove("LoggedInUserEmail");

            // Navigate back to the login page
            await Navigation.PushModalAsync(new LoginPage(_auth0Client, _mongoDBHelper));
        }
    }
}
