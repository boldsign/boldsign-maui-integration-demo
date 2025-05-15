using System.Text.Json.Serialization;

namespace BoldSignMauiIntegration.Services
{
    public record PDFResponse(
        [property: JsonPropertyName("file")] string File,
        [property: JsonPropertyName("expiration")] DateTime Expiration,
        [property: JsonPropertyName("pages")] int Pages,
        [property: JsonPropertyName("size")] int Size
    );
}
