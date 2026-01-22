using Newtonsoft.Json;

namespace Credit_Monitor.APIDto
{
    [JsonObject(MemberSerialization.OptOut)]
    public class FileDetailsResponseDto
    {
        [JsonProperty("$id")]
        public string Id { get; set; }

        [JsonProperty("$values")]
        public List<FileDetailDto> Documents { get; set; }
    }


    public class FileDetailDto
    {
        [JsonProperty("$id")]
        public string Id { get; set; }
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public string DocType { get; set; }
        public string ContentType { get; set; }
    }
}