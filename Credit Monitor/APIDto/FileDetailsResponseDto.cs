using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Credit_Monitor.APIDto
{
    public class FileResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        // We skip the 'FileData' wrapper and map directly to the list.
        // Newtonsoft with Metadata settings will "flatten" the $values into this list.
        [JsonProperty("data")]
        public List<FileItemDto> Values { get; set; } = new();
    }

    // For POST (returns just the new ID)
    public class SingleFileResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ActionData Data { get; set; } = new();
    }

    public class ActionData
    {
        public Guid Id { get; set; }
    }

    // For the Verification GET/POST request body
    public class VerifyFileRequest
    {
        public Guid FileId { get; set; }
        public bool IsVerified { get; set; }
    }

    public class FileItemDto
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string DocType { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;

        // Mapping the specific casing from your JSON
        [JsonPropertyName("isVeriFied")]
        public bool isVeriFied { get; set; }
    }

    public class DeleteFileResponseDto
    {  
        public bool Success { get; set; }
        public string? Message { get; set; }

    }
}