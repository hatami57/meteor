using System;
using System.Text.Json;
using NodaTime;

namespace Meteor.Utils.JsonConverters
{
    public class LocalTimeJsonConverter : System.Text.Json.Serialization.JsonConverter<LocalTime>
    {
        public override LocalTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString().ToLocalTime().Value;
        }

        public override void Write(Utf8JsonWriter writer, LocalTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToTimeString());
        }
    }
}