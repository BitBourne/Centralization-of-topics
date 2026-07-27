using HorusApp.ViewModels;


namespace HorusApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	// Método nativo para alternar la visibilidad
	private void OnTogglePasswordClicked(object sender, EventArgs e)
	{
		// Cambiamos el estado de oculto/visible
		PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

		// Cambiamos el texto del botón dinámicamente
		if (sender is Button button)
		{
			button.Text = PasswordEntry.IsPassword ? "Ver" : "Ocultar";
		}
	}
}