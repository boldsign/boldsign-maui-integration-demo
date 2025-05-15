using BoldSignMauiIntegration.Views;

namespace BoldSignMauiIntegration
{
    public partial class App : Application
    {
        public App() => InitializeComponent();

        protected override Window CreateWindow(IActivationState? activationState) => new(new Documents(new ViewModels.DocumentsViewModel()));
    }
}