using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ServerAction
{
    public class RequestConsoleResponse
    {

        [JsonProperty("wss_url", NullValueHandling = NullValueHandling.Ignore)]
        public string Wss_Url { get; set; } = string.Empty;


        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; } = string.Empty;

        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public Action.Action Action { get; set; }

    }
}
