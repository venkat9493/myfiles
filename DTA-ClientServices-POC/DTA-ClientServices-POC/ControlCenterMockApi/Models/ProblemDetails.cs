using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ControlCenterMockApi.Models
{
   
    public class Extensions
    {
        [JsonPropertyName("additionalProp1")]
        public string AdditionalProp1 { get; set; }

        [JsonPropertyName("additionalProp2")]
        public string AdditionalProp2 { get; set; }

        [JsonPropertyName("additionalProp3")]
        public string AdditionalProp3 { get; set; }
    }

    public class ProblemDetails
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        [JsonPropertyName("instance")]
        public string Instance { get; set; }

        [JsonPropertyName("extensions")]
        public Extensions Extensions { get; set; }

        [JsonPropertyName("additionalProp1")]
        public string AdditionalProp1 { get; set; }

        [JsonPropertyName("additionalProp2")]
        public string AdditionalProp2 { get; set; }

        [JsonPropertyName("additionalProp3")]
        public string AdditionalProp3 { get; set; }
    }


}
