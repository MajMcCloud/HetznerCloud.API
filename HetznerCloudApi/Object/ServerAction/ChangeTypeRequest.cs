using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ServerAction
{
    public class ChangeTypeRequest
    {
        /// <summary>
        /// Indicates if the routes from this network should be exposed to the vSwitch connection.
        /// </summary>
        [JsonProperty("upgrade_disk", NullValueHandling = NullValueHandling.Ignore)]
        public bool Upgrade_Disk { get; set; } = false;

        /// <summary>
        /// Name of the Network
        /// </summary>
        [JsonProperty("server_type", NullValueHandling = NullValueHandling.Ignore)]
        public string Server_Type { get; set; } = string.Empty;


    }
}
