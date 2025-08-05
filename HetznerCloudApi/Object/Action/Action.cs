using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HetznerCloudApi.Object.Action
{
    [DebuggerDisplay("{Command}: {Status}")]
    public partial class Action
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long Id { get; set; } = 0;

        [JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)]
        public string Command { get; set; } = string.Empty;

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public eActionStatus Status { get; set; } = eActionStatus.Unknown;

        [JsonProperty("progress", NullValueHandling = NullValueHandling.Ignore)]
        public long Progress { get; set; } = 0;

        [JsonProperty("started", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Started { get; set; } = new DateTime(1900, 01, 01);

        [JsonProperty("finished", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Finished { get; set; } = new DateTime(1900, 01, 01);

        [JsonProperty("resources", NullValueHandling = NullValueHandling.Ignore)]
        public List<Resource> Resources { get; set; } = new List<Resource>();

        //[JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        //public Error Error { get; set; } = new Error();
    }

    public class Resource
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long Id { get; set; } = 0;

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; } = string.Empty;
    }

    [DebuggerDisplay("{Action.Command}: {Action.Status}")]
    public class SimpleActionResponse
    {
        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public Action Action { get; set; } = new Action();
    }

    public enum eActionStatus
    {
        Unknown,
        Running,
        Success,
        Error,
    }
}