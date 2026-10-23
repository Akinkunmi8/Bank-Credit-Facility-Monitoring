using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Credit_Monitor.APIDto
{
    public class ResolutionResponseWrapper
    {
        [JsonProperty("$values")]
        public List<ExistingResolutionDto> DataList { get; set; } = new();

        [JsonProperty("$id")]
        public string? Id { get; set; }
    }

    public class ExistingResolutionDto
    {
        [JsonProperty("id")]
        public Guid? Id { get; set; }

        [JsonProperty("appId")]
        public Guid? AppId { get; set; }

        [JsonProperty("discrepancies")]
        public string? Discrepancies { get; set; }

        [JsonProperty("remarks")]
        public string? Remarks { get; set; }

        [JsonProperty("recomendedAction")]
        public string? RecomendedAction { get; set; }

        [JsonProperty("responsibility")]
        public string? Responsibility { get; set; }

        // RM/AO response
        [JsonProperty("resolutionAction")]
        public string? ResolutionAction { get; set; }

        [JsonProperty("resolutionRemarks")]
        public string? ResolutionRemarks { get; set; }

        [JsonProperty("respondedBy")]
        public string? RespondedBy { get; set; }

        [JsonProperty("respondedAt")]
        public DateTime? RespondedAt { get; set; }
    }
}