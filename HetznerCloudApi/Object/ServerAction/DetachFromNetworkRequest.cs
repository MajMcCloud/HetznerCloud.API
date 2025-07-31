using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ServerAction
{
    public class DetachFromNetworkRequest
    {
        /// <summary>
        /// Name of the Network
        /// </summary>
        [JsonProperty("network", NullValueHandling = NullValueHandling.Ignore)]
        public long Network { get; set; }


    }
}
