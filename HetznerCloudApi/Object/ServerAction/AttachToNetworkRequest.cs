using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ServerAction
{
    public class AttachToNetworkRequest
    {
        /// <summary>
        /// Name of the Network
        /// </summary>
        [JsonProperty("network", NullValueHandling = NullValueHandling.Ignore)]
        public long Network { get; set; }

        /// <summary>
        /// Name of the Network
        /// </summary>
        [JsonProperty("ip", NullValueHandling = NullValueHandling.Ignore)]
        public string IP { get; set; }

        /// <summary>
        /// Name of the Network
        /// </summary>
        [JsonProperty("alias_ips", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Alias_IPs { get; set; }

    }
}
