namespace HorusApp.Views;

using HorusApp.ViewModels;

public partial class AlertsPage : ContentPage
{
	private readonly AlertsViewModel _viewModel;

	public AlertsPage(AlertsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (_viewModel.LoadAlertsCommand.CanExecute(null))
		{
			await _viewModel.LoadAlertsCommand.ExecuteAsync(null);
		}
	}
}