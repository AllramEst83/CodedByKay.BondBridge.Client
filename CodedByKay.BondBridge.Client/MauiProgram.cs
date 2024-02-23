using CodedByKay.BondBridge.Client.Models;
using CodedByKay.BondBridge.Client.Pages;
using CodedByKay.BondBridge.Client.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

            builder.Services
                //Pages
                .AddSingleton<ConversationsListPage>()
                .AddSingleton<ConversationPage>()
                .AddSingleton<ThemeSelectorPage>()
                .AddSingleton<AboutPage>()

                //ViewModels
                .AddSingleton<ConversationsListViewModel>()
                .AddSingleton<ConverstaionViewModel>()
                .AddSingleton<ThemeSelectorViewModel>()
                .AddSingleton<AboutPageViewModel>();


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
