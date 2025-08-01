using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ServerAction
{
    public class CreateImageRequest
    {
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; } = string.Empty;


        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; } = "snapshot";


        [JsonProperty("labels", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();

    }
}
