using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using HorusApp.Services;
using HorusApp.ViewModels;
using HorusApp.Views;

namespace HorusApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            fonts.AddFont("FontAwesome.ttf", "FontAwesome");
        }).UseMauiCommunityToolkit();
        var apiBaseAddress = new Uri("http://192.168.100.236:5001/");
        // Client HTTP
        builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = apiBaseAddress });
        // Servicios MVVM
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();
        builder.Services.AddSingleton<AlertService>();
        // ViewModels
        builder.Services.AddTransient<AlertDetailViewModel>();
        builder.Services.AddTransient<AlertsViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LogoutViewModel>();   
        // Views / Pages
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<AlertDetailPage>();
        builder.Services.AddTransient<AlertsPage>();
        builder.Services.AddTransient<LoginPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}