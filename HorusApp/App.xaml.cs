using Microsoft.Extensions.DependencyInjection;

namespace HorusApp
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
			MainPage = new AppShell();
		}
	}
}