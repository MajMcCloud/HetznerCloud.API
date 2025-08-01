using HetznerCloudApi.Object.Universal;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HetznerCloudApi.Object.ISOs.Get
{
    public class Response
    {
        [JsonProperty("isos", NullValueHandling = NullValueHandling.Ignore)]
        public List<ISO> ISOs { get; set; } = new List<ISO>();

        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public Meta Meta { get; set; } = new Meta();
    }
}
