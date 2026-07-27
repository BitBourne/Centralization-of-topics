namespace HorusApp.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HorusApp.Services;
using HorusApp.Models;
using HorusApp.Views;

public partial class AlertsViewModel : ObservableObject
{
	private readonly AlertService _alertService;



	[ObservableProperty]
	private bool _isLoading;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasErrorMessage))]
	private string _errorMessage = string.Empty;
	// Esta propiedad devolverá true solo cuando ErrorMessage tenga texto
	public bool HasErrorMessage => !string.IsNullOrWhiteSpace(ErrorMessage);

	[ObservableProperty]
	private MobileAlertDto? _selectedAlert;

	public ObservableCollection<MobileAlertDto> Alerts { get; } = new();

	public AlertsViewModel(AlertService alertService)
	{
		_alertService = alertService;
	}

	[RelayCommand]
	public async Task LoadAlertsAsync()
	{
		try
		{
			IsLoading = true;
			ErrorMessage = string.Empty;

			var items = await _alertService.GetAlertsAsync();

			// Actualización de la UI
			MainThread.BeginInvokeOnMainThread(() =>
			{
				Alerts.Clear();
				if (items != null && items.Count > 0)
				{
					foreach (var item in items)
					{
						Alerts.Add(item);
					}
				}
			});
		}
		catch (Exception ex)
		{
			ErrorMessage = $"Error al cargar alertas: {ex.Message}";
		}
		finally
		{
			// Esto le indica al RefreshView que el proceso terminó y debe ocultar el spinner
			IsLoading = false;
		}
	}

	[RelayCommand]
	private async Task SelectAlertAsync(MobileAlertDto alert)
	{
		if (alert == null) return;

		// Pasamos el ID de la alerta como parámetro en la ruta de Shell
		var navigationParameters = new Dictionary<string, object>
		{
			{ "AlertId", alert.Id } 
		};

		await Shell.Current.GoToAsync(nameof(AlertDetailPage), navigationParameters);

		SelectedAlert = null;
	}
}