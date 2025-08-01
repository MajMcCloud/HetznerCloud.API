using HetznerCloudApi.Object.ServerType;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ISOs
{
    public class ISO
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("deprecation", NullValueHandling = NullValueHandling.Ignore)]
        public Deprecation Deprecation { get; set; }

        [JsonProperty("architecture", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public eArchitectureType Architecture { get; set; }
    }

    public class Deprecation
    {
        [JsonProperty("unavailable_after", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Unavailable_After { get; set; }

        [JsonProperty("announced", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Announced { get; set; }
    }
}
