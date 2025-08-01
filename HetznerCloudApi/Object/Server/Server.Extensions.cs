using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.Server
{
    public partial class Server
    {
        [JsonIgnore]
        public eServerStatus StatusEnum
        {
            get
            {
                return Enum.TryParse<eServerStatus>(Status, true, out var status) ? status : eServerStatus.Unknown;
            }
        }

    }
}
