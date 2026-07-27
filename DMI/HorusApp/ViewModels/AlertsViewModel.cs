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
	private string _errorMessage = string.Empty;

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
		if (IsLoading) return;

		try
		{
			IsLoading = true;
			ErrorMessage = string.Empty;

			var items = await _alertService.GetAlertsAsync();

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