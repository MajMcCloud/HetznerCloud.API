using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.ServerAction
{
    public class CreateImageResponse
    {
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Image.Image Image { get; set; }

        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public Action.Action Action { get; set; }

    }
}
