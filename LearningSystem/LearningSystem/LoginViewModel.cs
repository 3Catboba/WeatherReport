using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Auth0.OidcClient;
using LearningSystem.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace LearningSystem.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly Auth0Client _auth0Client;
        private readonly MongoDBHelper _mongoDBHelper;
        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand Auth0LoginCommand { get; }
        public ICommand SignUpCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel(Auth0Client auth0Client, MongoDBHelper mongoDBHelper)
        {
            _auth0Client = auth0Client;
            _mongoDBHelper = mongoDBHelper;

            LoginCommand = new Command(async () => await OnLoginClicked());
            Auth0LoginCommand = new Command(async () => await OnAuth0LoginClicked());
            SignUpCommand = new Command(async () => await OnSignUpTapped());
        }

        private async Task OnLoginClicked()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both email and password", "OK");
                return;
            }

            try
            {
                var user = await _mongoDBHelper.GetUserByEmailAsync(Email);

                if (user != null && user.Password == Password)
                {
                    // User found, login successful
                    Preferences.Set("LoggedInUserEmail", Email);
                    await Application.Current.MainPage.DisplayAlert("Success", "Login successful", "OK");
                    await Application.Current.MainPage.Navigation.PushModalAsync(new WeatherPage(_mongoDBHelper, _auth0Client));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task OnAuth0LoginClicked()
        {
            try
            {
                var loginResult = await _auth0Client.LoginAsync();
                if (loginResult.IsError)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", loginResult.ErrorDescription, "OK");
                    return;
                }

                var email = loginResult.User.FindFirst(c => c.Type == "email")?.Value;
                Preferences.Set("LoggedInUserEmail", email);
                await Application.Current.MainPage.DisplayAlert("Success", $"Welcome {loginResult.User.FindFirst(c => c.Type == "name")?.Value}", "OK");
                await Application.Current.MainPage.Navigation.PushModalAsync(new WeatherPage(_mongoDBHelper, _auth0Client));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task OnSignUpTapped()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SignupPage(_auth0Client, _mongoDBHelper));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
