using BoldSignMauiIntegration.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BoldSignMauiIntegration.ViewModels;

public partial class DocumentsViewModel : ObservableObject
{
    public DocumentsViewModel() => SignDocumentCommand = new AsyncRelayCommand(SignDocumentAsync);

    [ObservableProperty]
    private string _documentSinged;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _statusMessage;

    [ObservableProperty]
    private bool _isStatusVisible;

    public IAsyncRelayCommand SignDocumentCommand { get; }

    private async Task<string> SignDocumentAsync()
    {
        IsStatusVisible = false;

        string result = string.Empty;

        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email))
        {
            StatusMessage = "Please fill in all fields";

            IsStatusVisible = true;

            return string.Empty;
        }

        try
        {
            result = await BoldSignApiClient.SignDocumentAsync(Name, Email);

            StatusMessage = "Document signed successfully!";

            IsStatusVisible = true;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";

            IsStatusVisible = true;
        }

        return result;
    }
}
