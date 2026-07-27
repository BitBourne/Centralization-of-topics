using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HorusApp.Services;

namespace HorusApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
	private readonly IAuthService _authService;
	private readonly INotificationService _notificationService;

	[ObservableProperty]
	private string _username = string.Empty;

	[ObservableProperty]
	private string _password = string.Empty;

	[ObservableProperty]
	private bool _isLoading;

	public LoginViewModel(IAuthService authService, INotificationService notificationService)
	{
		_authService = authService;
		_notificationService = notificationService;
	}

	[RelayCommand]
	private async Task LoginAsync()
	{
		if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
		{
			await Shell.Current.DisplayAlert("Atención", "Por favor, llena todos los campos.", "OK");
			return;
		}

		IsLoading = true;
		try
		{
			var session = await _authService.LoginAsync(Username, Password);

			string mockFcmToken = "fcm_mock_token_test_2026_horus_android_client_local_network";
			var fcmResponse = await _notificationService.RegisterTokenAsync(mockFcmToken, session.Token, "Celular-Prueba-FCM");

			if (fcmResponse.Status == "registered")
			{
				await Shell.Current.DisplayAlert("FCM Conectado", $"Servidor Horus dice: {fcmResponse.Message}, {session.Token}, {mockFcmToken}", "Excelente");
			}
			else
			{
				await Shell.Current.DisplayAlert("FCM Alerta", $"El servidor rechazó el token: {fcmResponse.Message}", "Revisar");
			}

			await Shell.Current.GoToAsync("//AlertsPage");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Error de Autenticación / Red", ex.Message, "OK");
		}
		finally
		{
			IsLoading = false;
		}
	}
}