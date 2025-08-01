using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ServerAction
{
    public class ChangeReverseDNSEntryRequest
    {
        [JsonProperty("ip", NullValueHandling = NullValueHandling.Ignore)]
        public string IP { get; set; } = string.Empty;

        [JsonProperty("dns_ptr", NullValueHandling = NullValueHandling.Ignore)]
        public string DNS_Ptr { get; set; } = string.Empty;

    }
}
