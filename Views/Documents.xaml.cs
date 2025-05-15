using BoldSignMauiIntegration.ViewModels;

namespace BoldSignMauiIntegration.Views;

public partial class Documents : ContentPage
{
	public Documents(DocumentsViewModel viewModel)
	{
        InitializeComponent();

        BindingContext = viewModel;
	}
}