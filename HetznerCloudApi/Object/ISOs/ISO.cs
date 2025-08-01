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
        public int id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        [JsonProperty("deprecation", NullValueHandling = NullValueHandling.Ignore)]
        public Deprecation deprecation { get; set; }

        [JsonProperty("architecture", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public eArchitectureType architecture { get; set; }
    }

    public class Deprecation
    {
        [JsonProperty("unavailable_after", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime unavailable_after { get; set; }

        [JsonProperty("announced", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime announced { get; set; }
    }
}
