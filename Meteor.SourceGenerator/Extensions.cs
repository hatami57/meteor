using Microsoft.CodeAnalysis;

namespace Meteor.SourceGenerator
{
    public static class Extensions
    {
        public static void Report(this GeneratorExecutionContext context, DiagnosticDescriptor descriptor,
            params object?[]? messageArgs) =>
            context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None, messageArgs));
    }
}