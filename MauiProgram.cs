using BoldSignMauiIntegration.ViewModels;
using BoldSignMauiIntegration.Views;
using Microsoft.Extensions.Logging;

namespace BoldSignMauiIntegration
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();

            // Documents 
            builder.Services.AddTransient<DocumentsViewModel>();
            builder.Services.AddTransient<Documents>();
#endif
            return builder.Build();
        }
    }
}
