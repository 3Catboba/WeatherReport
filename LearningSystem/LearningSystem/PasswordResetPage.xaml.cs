using LearningSystem.Models;
using System.Net.Mail;
using System.Net;
using Auth0.OidcClient;

namespace LearningSystem;

public partial class PasswordResetPage : ContentPage
{
    private readonly MongoDBHelper _mongoDBHelper;
    private readonly Auth0Client _auth0Client;

    public PasswordResetPage(Auth0Client auth0Client, MongoDBHelper mongoDBHelper)
    {
        InitializeComponent();
        _mongoDBHelper = mongoDBHelper;
        _auth0Client = auth0Client;
    }

    private async void OnSendResetLinkClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;

        if (string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlert("Error", "Please enter your email address", "OK");
            return;
        }

        try
        {
            var user = await _mongoDBHelper.GetUserByEmailAsync(email);

            if (user != null)
            {
                string newPassword = GenerateRandomPassword();
                user.Password = newPassword;
                await _mongoDBHelper.SaveUserAsync(user);

                bool emailSent = await SendPasswordResetEmail(email, newPassword);

                if (emailSent)
                {
                    await DisplayAlert("Success", "A new password has been sent to your email", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to send the password reset email", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No account found with this email address", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private string GenerateRandomPassword()
    {
        Random random = new Random();
        return random.Next(10000, 99999).ToString();
    }

    private async Task<bool> SendPasswordResetEmail(string email, string newPassword)
    {
        try
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("dellyit001@gmail.com"),
                Subject = "Password Reset",
                Body = $"Your new password is: {newPassword}",
                IsBodyHtml = false
            };
            mail.To.Add(email);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 465,
                Credentials = new NetworkCredential("dellyit001@gmail.com", "mstqqigcrxjwqlsr"),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(mail);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    private async void OnBackToLoginTapped(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoginPage(_auth0Client, _mongoDBHelper));
    }
}
