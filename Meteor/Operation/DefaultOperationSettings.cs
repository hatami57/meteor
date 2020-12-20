namespace Meteor.Operation
{
    public static class DefaultOperationSettings
    {
        public static bool LogInput { get; set; }
        public static OperationLoggerAsync? LoggerAsync { get; set; }
    }
}