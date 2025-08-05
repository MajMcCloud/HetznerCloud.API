using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace HetznerCloudApi.Object.ServerType
{
    [DebuggerDisplay("#{Id}: {Description}, {CpuType}, {Cores}, {Memory} GB RAM")]
    public class ServerType
    {
        public static CultureInfo Culture = new CultureInfo("en-US");

        /// <summary>
        /// ID of the Server type
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long Id { get; set; } = 0;

        /// <summary>
        /// Unique identifier of the Server type
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the Server type
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Number of cpu cores a Server of this type will have
        /// </summary>
        [JsonProperty("cores", NullValueHandling = NullValueHandling.Ignore)]
        public long Cores { get; set; } = 0;

        /// <summary>
        /// Memory a Server of this type will have in GB
        /// </summary>
        [JsonProperty("memory", NullValueHandling = NullValueHandling.Ignore)]
        public double Memory { get; set; } = 0;

        /// <summary>
        /// Disk size a Server of this type will have in GB
        /// </summary>
        [JsonProperty("disk", NullValueHandling = NullValueHandling.Ignore)]
        public long Disk { get; set; } = 0;

        /// <summary>
        /// This field is deprecated. Use the deprecation object instead
        /// </summary>
        [JsonProperty("deprecated", NullValueHandling = NullValueHandling.Ignore)]
        public bool Deprecated { get; set; } = false;

        /// <summary>
        /// Prices in different Locations
        /// </summary>
        [JsonProperty("prices", NullValueHandling = NullValueHandling.Ignore)]
        public List<Price> Prices { get; set; } = new List<Price>();

        /// <summary>
        /// Possible enum values:
        /// localnetwork
        /// Type of Server boot drive. Local has higher speed. Network has better availability.
        /// </summary>
        [JsonProperty("storage_type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))] 
        public eStorageType StorageType { get; set; } = eStorageType.Unknown;

        /// <summary>
        /// Possible enum values:
        /// shareddedicated
        /// Type of cpu
        /// </summary>
        [JsonProperty("cpu_type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public eCpuType CpuType { get; set; } = eCpuType.Unknown;

        /// <summary>
        /// Possible enum values:
        /// x86arm
        /// Type of cpu architecture
        /// </summary>
        [JsonProperty("architecture", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public eArchitectureType Architecture { get; set; } = eArchitectureType.Unknown;

        /// <summary>
        /// Free traffic per month in bytes
        /// </summary>
        [JsonProperty("included_traffic", NullValueHandling = NullValueHandling.Ignore)]
        public double IncludedTraffic { get; set; }

        //[JsonProperty("deprecation")]
        //public object Deprecation { get; set; }

        public class Price
        {
            /// <summary>
            /// Name of the Location the price is for
            /// </summary>
            [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
            public string Location { get; set; } = string.Empty;

            /// <summary>
            /// Hourly costs for a Server type in this Location
            /// </summary>
            [JsonProperty("price_hourly", NullValueHandling = NullValueHandling.Ignore)]
            public PriceHourly PriceHourly { get; set; } = new PriceHourly();

            [JsonProperty("price_monthly", NullValueHandling = NullValueHandling.Ignore)]
            public PriceMonthly PriceMonthly { get; set; } = new PriceMonthly();
        }

        public class PriceHourly
        {
            /// <summary>
            /// Price without VAT
            /// </summary>
            [JsonProperty("net", NullValueHandling = NullValueHandling.Ignore)]
            public string Net { get; set; } = string.Empty;

            /// <summary>
            /// Price with VAT added
            /// </summary>
            [JsonProperty("gross", NullValueHandling = NullValueHandling.Ignore)]
            public string Gross { get; set; } = string.Empty;

            public decimal NetDecimal
            {
                get
                {
                    return decimal.TryParse(Net, NumberStyles.Number, ServerType.Culture, out var value) ? value : 0;
                }
            }
            public decimal GrossDecimal
            {
                get
                {
                    return decimal.TryParse(Gross, NumberStyles.Number, ServerType.Culture, out var value) ? value : 0;
                }
            }
        }

        public class PriceMonthly
        {
            /// <summary>
            /// Price without VAT
            /// </summary>
            [JsonProperty("net", NullValueHandling = NullValueHandling.Ignore)]
            public string Net { get; set; } = string.Empty;

            /// <summary>
            /// Price with VAT added
            /// </summary>
            [JsonProperty("gross", NullValueHandling = NullValueHandling.Ignore)]
            public string Gross { get; set; } = string.Empty;

            public decimal NetDecimal
            {
                get
                {
                    return decimal.TryParse(Net, NumberStyles.Number, ServerType.Culture, out var value) ? value : 0;
                }
            }
            public decimal GrossDecimal
            {
                get
                {
                    return decimal.TryParse(Gross, NumberStyles.Number, ServerType.Culture, out var value) ? value : 0;
                }
            }
        }
    }

    public enum eStorageType
    {
        Unknown,
        Local,
        Network
    }
    public enum eCpuType
    {
        Unknown,
        Shared,
        Dedicated
    }
    public enum  eArchitectureType
    {
        Unknown,
        X86,
        Arm
    }
}
