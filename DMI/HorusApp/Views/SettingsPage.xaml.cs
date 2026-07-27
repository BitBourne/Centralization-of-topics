namespace HorusApp.Views;

using HorusApp.ViewModels;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(LogoutViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}