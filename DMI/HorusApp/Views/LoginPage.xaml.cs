using HorusApp.ViewModels;

namespace HorusApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private void OnTogglePasswordClicked(object sender, EventArgs e)
	{
		// Alternar visibilidad de la contraseña
		PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

		// Actualizar el texto del botón dinámicamente
		if (sender is Button button)
		{
			button.Text = PasswordEntry.IsPassword ? "Ver" : "Ocultar";
		}
	}
}