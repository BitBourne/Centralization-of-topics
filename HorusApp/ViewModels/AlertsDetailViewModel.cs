namespace HorusApp.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HorusApp.Services;
using HorusApp.Models;

[QueryProperty(nameof(AlertId), "AlertId")]
public partial class AlertDetailViewModel : ObservableObject
{
	private readonly AlertService _alertService;

	public AlertDetailViewModel(AlertService alertService)
	{
		_alertService = alertService;
	}

	[ObservableProperty]
	private string _alertId = string.Empty;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsNotLoading))] // Notifica a IsNotLoading cuando cambia IsLoading
	private bool _isLoading;

	// Propiedad limpia para evitar Converters en XAML
	public bool IsNotLoading => !IsLoading;

	[ObservableProperty]
	private MobileAlertDto? _alert;

	partial void OnAlertIdChanged(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			_ = LoadAlertDetailAsync(value);
		}
	}

	[RelayCommand]
	private async Task LoadAlertDetailAsync(string id)
	{
		if (IsLoading) return;

		try
		{
			IsLoading = true;

			// Asignamos la alerta obtenida de la API
			Alert = await _alertService.GetAlertByIdAsync(id);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error al cargar detalle de alerta: {ex.Message}");
		}
		finally
		{
			IsLoading = false;
		}
	}

	[RelayCommand]
	private async Task GoBackAsync()
	{
		await Shell.Current.GoToAsync("..");
	}
}