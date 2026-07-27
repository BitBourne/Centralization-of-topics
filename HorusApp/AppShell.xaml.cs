using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using HorusApp.Views;
using Font = Microsoft.Maui.Font;

namespace HorusApp
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(AlertDetailPage), typeof(AlertDetailPage));
		}
	}
}
