using Microsoft.CodeAnalysis;

namespace Meteor.SourceGenerator.Dto
{
    internal class OperationDescriptor
    {
        public OperationDescriptor(INamedTypeSymbol classSymbol)
        {
            ClassSymbol = classSymbol;
        }
        
        public INamedTypeSymbol ClassSymbol { get; set; }
        public string? OperationType { get; set; }
        public string? InputType { get; set; }
        public string? OutputType { get; set; }
        public string? InputProperties { get; set; }
        public string? OutputProperties { get; set; }
        public string? InputClassName { get; set; }
        public string? OutputClassName { get; set; }
        public string? InputBaseInterface { get; set; }
        public string? OutputBaseInterface { get; set; }
    }
}