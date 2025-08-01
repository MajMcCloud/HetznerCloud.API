using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.PrimaryIPs
{
    public class PrimaryIPCreateRequest
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("labels", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Labels { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("datacenter", NullValueHandling = NullValueHandling.Ignore)]
        public string Datacenter { get; set; }

        [JsonProperty("assignee_type", NullValueHandling = NullValueHandling.Ignore)]
        public string AssigneeType { get; set; }

        [JsonProperty("assignee_id", NullValueHandling = NullValueHandling.Ignore)]
        public long AssigneeId { get; set; }

        [JsonProperty("auto_delete", NullValueHandling = NullValueHandling.Ignore)]
        public bool AutoDelete { get; set; }
    }
}
