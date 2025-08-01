using HetznerCloudApi.Object.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.Action
{
    public partial class Action
    {
        [JsonIgnore]
        public eActionStatus StatusEnum
        {
            get
            {
                return Enum.TryParse<eActionStatus>(Status, true, out var status) ? status : eActionStatus.Unknown;
            }
        }



    }
}
