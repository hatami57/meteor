using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Meteor.Logger.PostgreSqlSink
{
    public class PostgreSqlSink : PeriodicBatchingSink
    {
        private readonly string _connectionString;

        private readonly string _fullTableName;
        private readonly IDictionary<string, ColumnWriterBase> _columnOptions;
        private readonly IFormatProvider _formatProvider;
        private readonly bool _useCopy;

        public const int DefaultBatchSizeLimit = 30;
        public const int DefaultQueueLimit = int.MaxValue;

        private bool _isTableCreated;


        public PostgreSqlSink(string connectionString,
            string tableName,
            TimeSpan period,
            IFormatProvider formatProvider = null,
            IDictionary<string, ColumnWriterBase> columnOptions = null,
            int batchSizeLimit = DefaultBatchSizeLimit,
            bool useCopy = true,
            string schemaName = "",
            bool needAutoCreateTable = false,
            bool respectCase = false) : base(batchSizeLimit, period)
        {
            _connectionString = connectionString;

            if (respectCase)
            {
                schemaName = QuoteIdentifier(schemaName);
                tableName = QuoteIdentifier(tableName);
            }

            _fullTableName = GetFullTableName(tableName, schemaName);

            _formatProvider = formatProvider;
            _useCopy = useCopy;

            _columnOptions = columnOptions ?? ColumnOptions.Default;
            if (respectCase)
            {
                _columnOptions = CreateQuotedColumnsDict(_columnOptions);
            }


            _isTableCreated = !needAutoCreateTable;
        }

        public PostgreSqlSink(string connectionString,
            string tableName,
            TimeSpan period,
            IFormatProvider formatProvider = null,
            IDictionary<string, ColumnWriterBase> columnOptions = null,
            int batchSizeLimit = DefaultBatchSizeLimit,
            int queueLimit = DefaultQueueLimit,
            bool useCopy = true,
            string schemaName = "",
            bool needAutoCreateTable = false,
            bool respectCase = false) : base(batchSizeLimit, period, queueLimit)
        {
            _connectionString = connectionString;

            if (respectCase)
            {
                schemaName = QuoteIdentifier(schemaName);
                tableName = QuoteIdentifier(tableName);
            }

            _fullTableName = GetFullTableName(tableName, schemaName);

            _formatProvider = formatProvider;
            _useCopy = useCopy;

            _columnOptions = columnOptions ?? ColumnOptions.Default;
            if (respectCase)
            {
                _columnOptions = CreateQuotedColumnsDict(_columnOptions);
            }


            _isTableCreated = !needAutoCreateTable;
        }

        private static string QuoteIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier) || identifier.StartsWith("\""))
            {
                return identifier;
            }

            return $"\"{identifier}\"";
        }

        private IDictionary<string, ColumnWriterBase> CreateQuotedColumnsDict(
            IDictionary<string, ColumnWriterBase> originalColumnsDict)
        {
            var result = new Dictionary<string, ColumnWriterBase>(originalColumnsDict.Count);

            foreach (var (key, value) in originalColumnsDict)
            {
                result[QuoteIdentifier(key)] = value;
            }

            return result;
        }

        private static string GetFullTableName(string tableName, string schemaName)
        {
            var schemaPrefix = string.Empty;
            if (!string.IsNullOrEmpty(schemaName))
            {
                schemaPrefix = schemaName + ".";
            }

            return schemaPrefix + tableName;
        }


        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            if (!_isTableCreated)
            {
                TableCreator.CreateTable(connection, _fullTableName, _columnOptions);
                _isTableCreated = true;
            }

            if (_useCopy)
            {
                ProcessEventsByCopyCommand(events, connection);
            }
            else
            {
                await ProcessEventsByInsertStatements(events, connection);
            }
        }

        private async Task ProcessEventsByInsertStatements(IEnumerable<LogEvent> events, NpgsqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = GetInsertQuery();

                foreach (var col in _columnOptions)
                {
                    command.Parameters.Add(ClearColumnNameForParameterName(col.Key), col.Value.DbType);
                }
                foreach (var logEvent in events)
                {
                    foreach (var col in _columnOptions)
                    {
                        command.Parameters[col.Key].Value = col.Value.GetValue(logEvent, _formatProvider);
                    }
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private static string ClearColumnNameForParameterName(string columnName)
        {
            return columnName?.Replace("\"", "");
        }

        private void ProcessEventsByCopyCommand(IEnumerable<LogEvent> events, NpgsqlConnection connection)
        {
            using var writer = connection.BeginBinaryImport(GetCopyCommand());
            foreach (var e in events)
            {
                writer.StartRow();

                foreach (var columnKey in _columnOptions.Keys)
                {
                    writer.Write(_columnOptions[columnKey].GetValue(e, _formatProvider),
                        _columnOptions[columnKey].DbType);
                }
            }

            writer.Complete();
        }

        private string GetCopyCommand()
        {
            var columns = string.Join(", ", _columnOptions.Keys);

            return $"COPY {_fullTableName}({columns}) FROM STDIN BINARY;";
        }

        private string GetInsertQuery()
        {
            var columns = string.Join(", ", _columnOptions.Keys);

            var parameters = string.Join(", ",
                _columnOptions.Keys.Select(cn => ":" + ClearColumnNameForParameterName(cn)));

            return $@"INSERT INTO {_fullTableName} ({columns})
                                        VALUES ({parameters})";
        }

        private void WriteToStream(NpgsqlBinaryImporter writer, IEnumerable<LogEvent> entities)
        {
        }
    }
}