using HorusApp.ViewModels;




namespace HorusApp.Views;

public partial class AlertDetailPage : ContentPage
{
	public AlertDetailPage(AlertDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}