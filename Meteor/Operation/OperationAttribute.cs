using System;

namespace Meteor.Operation
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class OperationAttribute : Attribute
    {
        public Type Type { get; set; }
        public object? InputProperties { get; }
        public object? OutputProperties { get; }
        public string? InputClassName { get; set; }
        public string? OutputClassName { get; set; }
        public Type? InputBaseInterface { get; set; }
        public Type? OutputBaseInterface { get; set; }

        public OperationAttribute(object? input = null, object? output = null)
        {
            Type = typeof(OperationAsync<,>);
            InputProperties = input;
            OutputProperties = output;
        }
    }
}