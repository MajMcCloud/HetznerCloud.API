using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.PrimaryIPs
{
    public class PrimaryIPCreatedResponse
    {
        [JsonProperty("primary_ip", NullValueHandling = NullValueHandling.Ignore)]
        public PrimaryIP Primary_IP { get; set; } = new PrimaryIP();

        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public Action.Action Action { get; set; } = new Action.Action();

    }
}
