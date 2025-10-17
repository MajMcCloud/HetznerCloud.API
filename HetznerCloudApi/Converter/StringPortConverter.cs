using Newtonsoft.Json;
using System;

namespace HetznerCloudApi.Converter
{
    /// <summary>
    /// Konvertiert einen String wie "80" oder "80-443" in ein PortRange-Objekt.
    /// </summary>
    public class StringPortConverter : JsonConverter<HetznerCloudApi.Object.Firewall.PortRange>
    {
        public override HetznerCloudApi.Object.Firewall.PortRange ReadJson(JsonReader reader, Type objectType, HetznerCloudApi.Object.Firewall.PortRange existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var parts = value.Split('-');
            if (parts.Length == 2)
            {
                if (long.TryParse(parts[0], out long from) && long.TryParse(parts[1], out long to))
                {
                    return new HetznerCloudApi.Object.Firewall.PortRange { From = from, To = to };
                }
            }
            else if (parts.Length == 1)
            {
                if (long.TryParse(parts[0], out long port))
                {
                    return new HetznerCloudApi.Object.Firewall.PortRange { From = port, To = port };
                }
            }
            throw new JsonSerializationException("Ungültiges Port-Format: " + value);
        }

        public override void WriteJson(JsonWriter writer, HetznerCloudApi.Object.Firewall.PortRange value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            if (value.From == value.To)
                writer.WriteValue(value.From.ToString());
            else
                writer.WriteValue($"{value.From}-{value.To}");
        }
    }
}
