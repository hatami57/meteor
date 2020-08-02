using System;
using System.Text.Json;
using NodaTime;

namespace Meteor.Utils.JsonConverters
{
    public class InstantJsonConverter : System.Text.Json.Serialization.JsonConverter<Instant>
    {
        public override Instant Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString().ToInstant().Value;
        }

        public override void Write(Utf8JsonWriter writer, Instant value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToInstantString());
        }
    }
}