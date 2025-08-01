using HetznerCloudApi.Object.Universal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HetznerCloudApi.Object.PrimaryIPs
{
    [DebuggerDisplay("{Id} - {Name}: {IP}")]
    public class PrimaryIP
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        //[JsonProperty("labels", NullValueHandling = NullValueHandling.Ignore)]
        //public Labels labels { get; set; }

        [JsonProperty("created", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Created { get; set; }

        [JsonProperty("blocked", NullValueHandling = NullValueHandling.Ignore)]
        public bool Blocked { get; set; }

        [JsonProperty("datacenter", NullValueHandling = NullValueHandling.Ignore)]
        public Datacenter.Datacenter Datacenter { get; set; }

        [JsonProperty("ip", NullValueHandling = NullValueHandling.Ignore)]
        public string IP { get; set; }

        [JsonProperty("dns_ptr", NullValueHandling = NullValueHandling.Ignore)]
        public List<DnsPtr> Dns_Ptr { get; set; }

        [JsonProperty("protection", NullValueHandling = NullValueHandling.Ignore)]
        public Protection Protection { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("auto_delete", NullValueHandling = NullValueHandling.Ignore)]
        public bool Auto_Delete { get; set; }

        [JsonProperty("assignee_type", NullValueHandling = NullValueHandling.Ignore)]
        public string Assignee_Type { get; set; }

        [JsonProperty("assignee_id", NullValueHandling = NullValueHandling.Ignore)]
        public int Assignee_Id { get; set; }



    }

    public class DnsPtr
    {
        [JsonProperty("ip", NullValueHandling = NullValueHandling.Ignore)]
        public string IP { get; set; }

        [JsonProperty("dns_ptr", NullValueHandling = NullValueHandling.Ignore)]
        public string Dns_Ptr { get; set; }
    }
}
