using System;
using System.Text.Json;
using NodaTime;

namespace Meteor.Utils.JsonConverters
{
    public class LocalDateJsonConverter : System.Text.Json.Serialization.JsonConverter<LocalDate>
    {
        public override LocalDate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString().ToLocalDate().Value;
        }

        public override void Write(Utf8JsonWriter writer, LocalDate value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToDateString());
        }
    }
}