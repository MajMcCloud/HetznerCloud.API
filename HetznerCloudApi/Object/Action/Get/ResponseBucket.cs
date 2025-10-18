using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.Action.Get
{
    public class ResponseBucket<T>
    {
        [JsonProperty("actions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Action> Actions { get; set; } = new List<Object.Action.Action>();

        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public Action Action { get; set; } = new Action();

        [JsonExtensionData]
        // Dieses Dictionary nimmt beliebige Objekttypen auf
        // z. B. "firewall": FirewallObject
        public Dictionary<string, JToken> Objects { get; set; }

        public T Response
        {
            get
            {
                // Wenn das Dictionary leer ist, geben wir den Standardwert für T zurück
                if (Objects == null || Objects.Count == 0)
                {
                    return default(T);
                }
                // Ansonsten geben wir das erste Objekt im Dictionary zurück
                foreach (var obj in Objects.Values)
                {
                    return obj.ToObject<T>(); // Rückgabe des ersten Werts
                }
                return default(T); // Fallback, sollte nie erreicht werden
            }
        }


        public T2 GetObject<T2>(String name)
        {
            if (Objects != null && Objects.ContainsKey(name))
            {
                return Objects[name].ToObject<T2>();
            }
            return default(T2);
        }

    }
}
