using HetznerCloudApi.Object.Universal;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HetznerCloudApi.Object.PrimaryIPs.Get
{
    public class Response
    {
        [JsonProperty("primary_ips", NullValueHandling = NullValueHandling.Ignore)]
        public List<PrimaryIP> Primary_IPs { get; set; } = new List<PrimaryIP>();

        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public Meta Meta { get; set; } = new Meta();
    }
}
