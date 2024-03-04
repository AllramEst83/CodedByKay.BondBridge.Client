using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.Models;
using CodedByKay.BondBridge.Client.Pages;
using CodedByKay.BondBridge.Client.Services;
using CodedByKay.BondBridge.Client.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using System.Diagnostics;


namespace CodedByKay.BondBridge.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //Appsettings configuration
            var assembly = typeof(MauiProgram).Assembly;
            string configFileName = string.Empty;

#if DEBUG
            configFileName = "CodedByKay.BondBridge.Client.appsettings.Local.json";
#else
            configFileName = "CodedByKay.BondBridge.Client.appsettings.json";
#endif

            LoadConfiguration(builder);

            builder.Services.AddOptions<ApplicationSettings>()
                    .Bind(builder.Configuration.GetSection("ApplicationSettings"));

            //HttpClinets
            builder.Services.AddHttpClient("BondBridgeClient", client =>
            {
                var appSettings = builder.Configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
                if (appSettings == null)
                {
                    throw new NullReferenceException("AppSettings can not be null.");
                }

                client.BaseAddress = new Uri(appSettings.BondBridgeApiBaseUrl);
                client.DefaultRequestHeaders.Add("CodedByKay-BondBridge-header", appSettings.CustomHeader);
            });

            builder.Services
                //Services
                .AddSingleton(SecureStorage.Default)
                .AddSingleton<IUserSecureStorageService, UserSecureStorageService>()
                .AddSingleton<IPreferencesService, PreferencesService>()
                .AddSingleton<IAuthenticationService, AuthenticationService>()
                .AddSingleton<IBondBridgeDataService, BondBridgeDataService>()

                //Pages
                .AddSingleton<SigninPage>()
                .AddSingleton<ConversationsListPage>()
                .AddSingleton<ConversationPage>()
                .AddSingleton<ThemeSelectorPage>()
                .AddSingleton<AboutPage>()
                .AddSingleton<RegistrationPage>()
                .AddSingleton<AccountDetailsPage>()
                .AddSingleton<LogPage>()
                .AddSingleton<LogDetailsPage>()

                //ViewModels
                .AddTransient<SigninViewModel>()
                .AddSingleton<ConversationsListViewModel>()
                .AddSingleton<ConverstaionViewModel>()
                .AddSingleton<ThemeSelectorViewModel>()
                .AddSingleton<AboutPageViewModel>()
                .AddSingleton<RegistrationViewModel>()
                .AddSingleton<AccountDetailsViewModel>()
                .AddSingleton<LogViewModel>()
                .AddSingleton<LogDetailsViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void LoadConfiguration(MauiAppBuilder builder)
        {
            var configFileName = GetConfigurationFileName();
            var assembly = typeof(MauiProgram).Assembly;

            using (var stream = assembly.GetManifestResourceStream(configFileName))
            {
                if (stream == null) throw new FileNotFoundException("Could not find configuration file.", configFileName);

                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                builder.Configuration.AddConfiguration(config);
            }
        }

        private static string GetConfigurationFileName()
        {
            return typeof(MauiProgram).Assembly.GetName().Name +
                   (Debugger.IsAttached ? ".appsettings.Local.json" : ".appsettings.json");
        }
    }
}
