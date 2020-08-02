using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Meteor.Logger.PostgreSqlSink
{
    public static class LoggerConfigurationPostgreSqlExtensions
    {
        /// <summary>
        /// Default time to wait between checking for event batches.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Adds a sink which writes to PostgreSQL table
        /// </summary>
        /// <param name="sinkConfiguration">The logger configuration.</param>
        /// <param name="connectionString">The connection string to the database where to store the events.</param>
        /// <param name="tableName">Name of the table to store the events in.</param>
        /// <param name="columnOptions">Table columns writers</param>
        /// <param name="addRequestIdColumn">Whether to add RequestId or not.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="batchSizeLimit">The maximum number of events to include to single batch.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <param name="useCopy">If true inserts data via COPY command, otherwise uses INSERT INTO satement </param>
        /// <param name="schemaName">Schema name</param>
        /// <param name="needAutoCreateTable">Set if sink should create table</param>
        /// <param name="respectCase">Set if sink should auto quotate identifiers </param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        public static LoggerConfiguration PostgreSql(this LoggerSinkConfiguration sinkConfiguration,
            string connectionString = null,
            string tableName = "system_log",
            IDictionary<string, ColumnWriterBase> columnOptions = null,
            bool addRequestIdColumn = true,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            TimeSpan? period = null,
            IFormatProvider formatProvider = null,
            int batchSizeLimit = PostgreSqlSink.DefaultBatchSizeLimit,
            LoggingLevelSwitch levelSwitch = null,
            bool useCopy = true,
            string schemaName = "public",
            bool needAutoCreateTable = false,
            bool respectCase = false)
        {
            if (sinkConfiguration == null)
            {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }
            
            period ??= DefaultPeriod;
            
            connectionString ??= EnvVars.Get<string>(EnvVarKeys.LogDbUri);
            columnOptions ??= ColumnOptions.Default;
            if (addRequestIdColumn && !columnOptions.ContainsKey("request_id"))
                columnOptions.Add("request_id", new RequestIdColumnWriter());

            return sinkConfiguration.Sink(new PostgreSqlSink(connectionString,
                tableName,
                period.Value,
                formatProvider,
                columnOptions,
                batchSizeLimit,
                useCopy,
                schemaName,
                needAutoCreateTable,
                respectCase), restrictedToMinimumLevel, levelSwitch);
        }
    }
}