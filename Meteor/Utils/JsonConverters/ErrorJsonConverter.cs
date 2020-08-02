using System;
using System.Text.Json;

namespace Meteor.Utils.JsonConverters
{
    public class ErrorJsonConverter : System.Text.Json.Serialization.JsonConverter<Error>
    {
        public override Error Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var code = 0;
            string message = null;
            object details = null;
            
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.EndObject:
                        return new Error(code, message, details);
                    case JsonTokenType.PropertyName:
                        var propName = reader.GetString();
                        reader.Read();
                        switch (propName)
                        {
                            case "code":
                                code = reader.GetInt32();
                                break;
                            case "message":
                                message = reader.GetString();
                                break;
                            case "details":
                                details = JsonSerializer.Deserialize<object>(ref reader, options).ToJson();
                                break;
                        }

                        break;
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, Error value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("code", value.Code);
            writer.WriteString("message", value.Message);
            
            if (value.Details != null)
            {
                writer.WritePropertyName("details");
                JsonSerializer.Serialize(writer, value.Details, options);
            }

            writer.WriteEndObject();
        }
    }

}
