using BoldSign.Api;
using BoldSign.Model;
using BoldSignMauiIntegration.Extensions;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BoldSignMauiIntegration.Services;

public static class BoldSignApiClient
{
    private static async Task<string> GetGeneratedPDFDownloadLinkAsync()
    {
        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.aPDF_AUTHORIZATION_SCHEME, Constants.aPDF_TOKEN);

        FormUrlEncodedContent requestData = new(
        [
            new KeyValuePair<string, string>("html", Constants.aPDF_TEMPLATE_HTML)
        ]);

        HttpResponseMessage response = await httpClient.PostAsync(Constants.aPDF_CREATE_PDF_ENDPOINT, requestData);

        PDFResponse? deserializedResponse = JsonSerializer.Deserialize<PDFResponse>(await response.Content.ReadAsStreamAsync());

        (bool isValid, string correctedUrl)? isUrlValid = deserializedResponse?.File.ValidateAndCorrectUrl();

        if (isUrlValid.Value.isValid)
        {
            var path = await DownloadAndSaveFile(isUrlValid.Value.correctedUrl, "Freelance-contract.pdf");

            return path;
        }

        return string.Empty;
    }

    private static async Task<string> DownloadAndSaveFile(string fileUrl, string fileName)
    {
        string downloadsPath = FileSystem.AppDataDirectory;

        string filePath = Path.Combine(downloadsPath, fileName);

        HttpClient httpClient = new();

        byte[] fileBytes = await httpClient.GetByteArrayAsync(fileUrl);

        await File.WriteAllBytesAsync(filePath, fileBytes);
        
        return filePath;
    }

    public static async Task<string> SignDocumentAsync(string name, string email)
    {
        ApiClient apiClient = new(Constants.BOLDSIGN_BASE_URL, Constants.BOLDSIGN_API_KEY);

        DocumentClient documentClient = new(apiClient);

        string filePath = await GetGeneratedPDFDownloadLinkAsync();

        DocumentFilePath documentFilePath = new()
        {
            ContentType = "application/pdf",
            FilePath = filePath
        };

        FormField signatureField = new(
        id: "signature",
        isRequired: true,
        type: FieldType.Signature,
        pageNumber: 1,
        bounds: new Rectangle(570, 855, 150, 50));

        List<FormField> formFields = [signatureField];

        DocumentSigner signer = new()
        {
            Name = name,
            EmailAddress = email,
            FormFields = formFields
        };

        SendForSign sendForSign = new()
        {
            Message = "Please sign this document",
            Title = "Freelance Contract",
            Signers = [signer],
            Files = [documentFilePath]
        };

        DocumentCreated response = await documentClient.SendDocumentAsync(sendForSign);

        return response.DocumentId;
    }
}
