using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HorusApp.Services;

namespace HorusApp.ViewModels;

public partial class LogoutViewModel : ObservableObject
{
	private readonly INotificationService _notificationService;
	private readonly HttpClient _httpClient;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsNotProcessing))]
	private bool _isProcessing;

	public bool IsNotProcessing => !IsProcessing;

	[ObservableProperty]
	private string _statusMessage = string.Empty;

	public LogoutViewModel(INotificationService notificationService, HttpClient httpClient)
	{
		_notificationService = notificationService;
		_httpClient = httpClient;
	}

	[RelayCommand]
	private async Task ExecuteLogoutAsync()
	{
		if (IsProcessing) return;

		bool confirm = await Shell.Current.DisplayAlert("Cerrar Sesión", "¿Estás seguro?", "Sí", "Cancelar");
		if (!confirm) return;

		IsProcessing = true;
		StatusMessage = "Eliminando registro de dispositivo...";

		try
		{
			string currentJwtToken = await SecureStorage.Default.GetAsync("jwt_token") ?? "mock_jwt_token_for_testing_purposes_only";
			string currentFcmToken = "fcm_mock_token_test_2026_horus_android_client_local_network";

			StatusMessage = "Comunicando con el servidor...";
			var response = await _notificationService.UnregisterTokenAsync(currentFcmToken, currentJwtToken);

			if (response.Status == "unregistered")
			{
				await Shell.Current.DisplayAlert("Servidor Responde", $"Encontrado: {response.Found}", "OK");
			}
			else
			{
				await Shell.Current.DisplayAlert("Respuesta Servidor", $"Estado: {response.Status}", "OK");
			}

			SecureStorage.Default.Remove("jwt_token");
			_httpClient.DefaultRequestHeaders.Authorization = null;

			await Shell.Current.GoToAsync("//LoginPage");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Error de Red", ex.Message, "OK");
		}
		finally
		{
			IsProcessing = false;
			StatusMessage = string.Empty;
		}
	}
}