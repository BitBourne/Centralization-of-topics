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

	// Controla la visibilidad del overlay cuando el usuario presiona una alerta
	[ObservableProperty]
	private bool _isNavigating;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasErrorMessage))]
	private string _errorMessage = string.Empty;

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
		// Previene múltiples toques mientras se procesa la navegación
		if (alert == null || IsNavigating) return;

		try
		{
			IsNavigating = true;

			var navigationParameters = new Dictionary<string, object>
			{
				{ "AlertId", alert.Id }
			};

			await Shell.Current.GoToAsync(nameof(AlertDetailPage), navigationParameters);
		}
		catch (Exception ex)
		{
			ErrorMessage = $"Error al abrir detalle: {ex.Message}";
		}
		finally
		{
			IsNavigating = false;
			SelectedAlert = null; // Resetea el elemento seleccionado en la CollectionView
		}
	}
}