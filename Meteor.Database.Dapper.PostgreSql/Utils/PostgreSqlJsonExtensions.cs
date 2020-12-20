using Meteor.Database.Dapper.PostgreSql.Utils.JsonConverters;
using Meteor.Utils;

namespace Meteor.Database.Dapper.PostgreSql.Utils
{
    public static class PostgreSqlJsonExtensions
    {
        static PostgreSqlJsonExtensions()
        {
            JsonExtensions.DefaultJsonSerializerOptions.Converters.Add(new GeometryPointJsonConverter());
        }
    }
}