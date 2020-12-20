using Microsoft.CodeAnalysis;

namespace Meteor.SourceGenerator
{
    internal static class Errors
    {
        internal static readonly DiagnosticDescriptor InvalidInputError = new DiagnosticDescriptor("MO0001",
            title: "Invalid input",
            messageFormat: "Invalid input: '{0}'",
            category: "OperationGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);
        internal static readonly DiagnosticDescriptor InvalidOutputError = new DiagnosticDescriptor("MO0002",
            title: "Invalid output",
            messageFormat: "Invalid output: '{0}'",
            category: "OperationGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);
        internal static readonly DiagnosticDescriptor MustBeTopLevel = new DiagnosticDescriptor("MO0003",
            title: "Class must be top level",
            messageFormat: "Class must be top level: '{0}'",
            category: "OperationGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}