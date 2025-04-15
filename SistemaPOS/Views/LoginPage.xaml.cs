using ApplicationLayer.DTOs.Auth;
using ApplicationLayer.Interfaces.Identity;
using ApplicationLayer.Interfaces.Infrastructure;
using MediatR;
using static System.Formats.Asn1.AsnWriter;

namespace SistemaPOS.Views;

public partial class LoginPage : ContentPage
{
    private readonly IIdentityService identityService;
    private readonly IAuthenticatedUserService authenticatedUserService;
    private readonly IMediator mediator;

    public LoginPage(IIdentityService identityService, IAuthenticatedUserService authenticatedUserService, IMediator mediator)
    {
        InitializeComponent();
        this.identityService = identityService;
        this.authenticatedUserService = authenticatedUserService;
        this.mediator = mediator;
    }

    private async void LoginButton_Pressed(object sender, EventArgs e)
    {
        try
        {
            LoginButton.BackgroundColor = Color.FromArgb("#ba181b");

            string? username = UsernameEntry?.Text?.Trim();
            string? password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Por favor, ingrese su usuario y contraseña", "OK");
                }
                return;
            }

            var loginRequestData = new LoginRequestDto
            {
                Email = username,
                Password = password
            };

            var loginResult = await identityService.GetTokenAsync(loginRequestData);

            if (loginResult.IsSuccess)
            {
                await authenticatedUserService.SaveAuthDataAsync(
                    loginResult.Data!.Token,
                    loginResult.Data.Id,
                    loginResult.Data.Email
                );

                await Navigation.PushAsync(new MainPage(authenticatedUserService, App.Services, mediator));
            }
            else
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", loginResult.Message, "OK");
                }
            }
        }
        catch (Exception ex)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        finally
        {
            LoginButton.BackgroundColor = Color.FromArgb("#e63946");
        }
    }

    private void LoginButton_Released(object sender, EventArgs e)
    {
        LoginButton.BackgroundColor = Color.FromArgb("#e63946");
    }
}