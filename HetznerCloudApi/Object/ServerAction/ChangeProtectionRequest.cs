using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ServerAction
{
    public class ChangeProtectionRequest
    {
        /// <summary>
        /// If true, prevents the Server from being deleted (currently delete and rebuild attribute needs to have the same value).
        /// </summary>
        [JsonProperty("delete", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Delete { get; set; } = false;

        /// <summary>
        /// If true, prevents the Server from being rebuilt (currently delete and rebuild attribute needs to have the same value).
        /// </summary>
        [JsonProperty("rebuild", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Rebuild { get; set; } = false;
    }
}
