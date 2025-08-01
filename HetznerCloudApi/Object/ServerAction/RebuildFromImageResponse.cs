using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ServerAction
{
    public class RebuildFromImageResponse
    {

        [JsonProperty("root_password", NullValueHandling = NullValueHandling.Ignore)]
        public string RootPassword { get; set; } = string.Empty;

        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public Action.Action Action { get; set; }

    }
}
