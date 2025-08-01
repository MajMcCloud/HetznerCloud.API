using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ServerAction
{
    public class ChangeAliasIPRequest
    {
        [JsonProperty("network", NullValueHandling = NullValueHandling.Ignore)]
        public long Network { get; set; } = 0;

        [JsonProperty("alias_ips", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Alias_IPs { get; set; } = Array.Empty<string>();

    }
}
