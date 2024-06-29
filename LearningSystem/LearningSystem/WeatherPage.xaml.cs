using LearningSystem.Services;
using LearningSystem.Modal;
using LearningSystem.Models;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;
using Auth0.OidcClient; // Ensure this using directive is present

namespace LearningSystem;

public partial class WeatherPage : ContentPage
{
    public List<Modal.List> WeatherList;

    double latitude;
    double longitude;
    private readonly MongoDBHelper _mongoDBHelper;
    private readonly Auth0Client _auth0Client; // Add Auth0Client reference
    private UserRecord _currentUser;

    public WeatherPage(MongoDBHelper mongoDBHelper, Auth0Client auth0Client)
    {
        InitializeComponent();

        WeatherList = new List<Modal.List>();
        _mongoDBHelper = mongoDBHelper;
        _auth0Client = auth0Client; // Initialize Auth0Client
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        // Fetch user data and set background color
        await LoadUserData();

        if (_currentUser != null && !string.IsNullOrEmpty(_currentUser.DefaultCity))
        {
            await GetWeatherDataByCity(_currentUser.DefaultCity);
        }
        else
        {
            await GetLocation();
            await GetWeatherDataByLocation(latitude, longitude);
        }
    }

    private async Task LoadUserData()
    {
        // Fetch the current user data
        var email = Preferences.Get("LoggedInUserEmail", string.Empty);
        _currentUser = await _mongoDBHelper.GetUserByEmailAsync(email);

        if (_currentUser != null)
        {
            var backgroundColor = _currentUser.BackgroundColor;
            if (!string.IsNullOrEmpty(backgroundColor))
            {
                BackgroundColor = backgroundColor switch
                {
                    "Blue" => Colors.Blue,
                    "Green" => Colors.Green,
                    "Red" => Colors.Red,
                    "Yellow" => Colors.Yellow,
                    "Black" => Colors.Black,
                    "White" => Colors.White,
                    _ => Colors.White
                };
            }
            else
            {
                BackgroundColor = Colors.White; // Set default color to White
            }
        }
    }

    public async Task GetLocation()
    {
        var location = await Geolocation.GetLocationAsync();

        if (location != null)
        {
            latitude = location.Latitude;
            longitude = location.Longitude;
        }
        else
        {
            // Handle the case where location is null, e.g., show an error message to the user
            await DisplayAlert("Error", "Unable to retrieve location. Please check your location settings and try again.", "OK");
        }
    }

    private async void TapLocation_Tapped(object sender, EventArgs e)
    {
        await GetLocation();
        await GetWeatherDataByLocation(latitude, longitude);
    }

    public async Task GetWeatherDataByLocation(double latitude, double longitude)
    {
        var result = await AppService.GetWeather(latitude, longitude);

        UpdateUI(result);
    }

    private async void ImageButton_Cliked(object sender, EventArgs e)
    {
        var response = await DisplayPromptAsync(title: "", message: "", placeholder: "search weather by city", accept: "Search", cancel: "Cancel");
        if (response != null)
        {
            await GetWeatherDataByCity(response);
        }
    }

    public async Task GetWeatherDataByCity(string city)
    {
        var result = await AppService.GetWeatherByCity(city);

        UpdateUI(result);
    }

    public void UpdateUI(dynamic result)
    {
        WeatherList.Clear();
        foreach (var item in result.list)
        {
            WeatherList.Add(item);
        }

        CvWeather.ItemsSource = WeatherList;
        lblCity.Text = result.city.name;
        lblWeatherDescription.Text = result.list[0].weather[0].description;
        lblTemperature.Text = result.list[0].main.temperature + "\u00B0C";
        lblHumidity.Text = result.list[0].main.humidity + "%";
        lblWind.Text = result.list[0].wind.speed + "km/h";
        ImgWeatherIcon.Source = result.list[0].weather[0].fullIconUrl;
    }

    private async void OnAccountIconClicked(object sender, EventArgs e)
    {
        // Navigate to the Account Page with user's email
        var email = Preferences.Get("LoggedInUserEmail", string.Empty);

        if (!string.IsNullOrEmpty(email))
        {
            await Navigation.PushModalAsync(new AccountPage(_mongoDBHelper, _auth0Client, email));
        }
        else
        {
            await DisplayAlert("Error", "User email not found. Please log in again.", "OK");
        }
    }
}
