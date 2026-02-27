using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Credit_Monitor.APIDto
{
    // This matches your standard API response pattern
    public class ResolutionResponseWrapper
    {
        [JsonProperty("$values")]
        public List<ExistingResolutionDto> DataList { get; set; } = new();
        [JsonProperty("$id")]
        public string Id { get; set; }
    }

    public class ExistingResolutionDto
    {
        public Guid? Id { get; set; }
        public Guid appId { get; set; }
        public string discrepancies { get; set; }
        public string remarks { get; set; }
        public string recomendedAction { get; set; } // Note: Check if your API uses 'recomendedAction' (one 'm') vs 'recommentedAction'
        public string responsibility { get; set; }
    }
}