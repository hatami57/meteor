using System.Collections.Generic;

namespace Meteor.Database.Dapper.PostgreSql.Logger.PostgreSqlSink
{
    public class ColumnOptions
    {
        public static IDictionary<string, ColumnWriterBase> Default => new Dictionary<string, ColumnWriterBase>
        {
            {DefaultColumnNames.App, new AppColumnWriter()},
            {DefaultColumnNames.RenderedMessage, new RenderedMessageColumnWriter()},
            {DefaultColumnNames.MessageTemplate, new MessageTemplateColumnWriter()},
            {DefaultColumnNames.Level, new LevelColumnWriter()},
            {DefaultColumnNames.Exception, new ExceptionColumnWriter()},
            {DefaultColumnNames.Properties, new PropertiesColumnWriter()},
            {DefaultColumnNames.CreatedAt, new TimestampColumnWriter()}
        };
    }

    public static class DefaultColumnNames
    {
        public const string App = "app";
        public const string RenderedMessage = "message";
        public const string MessageTemplate = "message_template";
        public const string Level = "level";
        public const string Exception = "exception";
        public const string Properties = "properties";
        public const string CreatedAt = "created_at";
    }
}