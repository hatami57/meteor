using System;
using System.Linq;
using System.Text.Json;
using Meteor.Database.PostgreSql;
using NetTopologySuite.Geometries;

namespace Meteor.Utils.JsonConverters
{
    public class GeometryPointJsonConverter : System.Text.Json.Serialization.JsonConverter<Point>
    {
        public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            if (string.IsNullOrWhiteSpace(str))
                return null;
            var parts = str.Split(',').Select(x => Convert.ToDouble(x.Trim())).ToArray();
            
            return parts.Length < 2
                ? null
                : MeteorGeometryFactory.Default.CreatePoint(new Coordinate(parts[1], parts[0]));
        }

        public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue($"{value.Y},{value.X}");
        }
    }
}