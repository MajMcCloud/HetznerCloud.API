using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.Action.Get
{
    /// <summary>
    /// Container for a response of type <typeparamref name="T"/> along with associated actions and objects.
    /// </summary>
    /// <remarks>
    /// This class supports the following implicit conversions:
    /// <list type="bullet">
    /// <item><description><c>T value = responseBucket;</c> – Returns the response object.</description></item>
    /// <item><description><c>Action action = responseBucket;</c> – Returns the action object.</description></item>
    /// <item><description><c>List&lt;Action&gt; actions = responseBucket;</c> – Returns the list of actions.</description></item>
    /// <item><description><c>(Action action, T response) = responseBucket;</c> – Returns a tuple of action and response.</description></item>
    /// <item><description><c>(List&lt;Action&gt; actions, T response) = responseBucket;</c> – Returns a tuple of actions and response.</description></item>
    /// <item><description><c>Dictionary&lt;string, JToken&gt; dict = responseBucket;</c> – Returns the object dictionary.</description></item>
    /// </list>
    /// </remarks>
    /// <typeparam name="T">The type of the primary response object.</typeparam>
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

        public static implicit operator T(ResponseBucket<T> bucket)
        {
            return bucket.Response;
        }

        public static implicit operator Action(ResponseBucket<T> bucket)
        {
            return bucket.Action;
        }

        public static implicit operator List<Action>(ResponseBucket<T> bucket)
        {
            return bucket.Actions;
        }

        public static implicit operator (Action action, T response)(ResponseBucket<T> bucket)
        {
            return (bucket.Action, bucket.Response);
        }

        public static implicit operator (List<Action> actions, T response)(ResponseBucket<T> bucket)
        {
            return (bucket.Actions, bucket.Response);
        }

        public static implicit operator Dictionary<string, JToken>(ResponseBucket<T> bucket)
        {
            return bucket.Objects;
        }

        public void Deconstruct(out Action action, out T response)
        {
            action = this.Action;
            response = this.Response;
        }



    }
}
