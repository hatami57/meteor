using System.Text.Json;
using Meteor.Utils.JsonConverters;

namespace Meteor.Utils
{
    public static class JsonExtensions
    {
        public static JsonSerializerOptions DefaultJsonSerializerOptions { get; } = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new ErrorJsonConverter(),
                new LocalTimeJsonConverter(),
                new LocalDateJsonConverter(),
                new InstantJsonConverter()
            }
        };

        public static string ToJson(this object obj, JsonSerializerOptions? options = null)
        {
            options ??= DefaultJsonSerializerOptions;

            return JsonSerializer.Serialize(obj, options);
        }
        
        public static T FromJson<T>(this string json, JsonSerializerOptions? options = null)
        {
            options ??= DefaultJsonSerializerOptions;

            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}