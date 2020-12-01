using System;
using System.Linq;
using System.Text;
using Meteor.SourceGenerator.Dto;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Meteor.SourceGenerator
{
    [Generator]
    public class OperationGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new OperationReceiver());
        }

        public void Execute(GeneratorExecutionContext ctx)
        {
            if (ctx.SyntaxReceiver is not OperationReceiver syntaxReceiver)
                return;

            var operationAttributeSymbol = ctx.Compilation.GetTypeByMetadataName("Meteor.Operation.OperationAttribute");
            if (operationAttributeSymbol == null)
                return;

            var operationDescriptors = syntaxReceiver.CandidateClasses
                .Select(x => GetOperationDescriptor(ctx, x, operationAttributeSymbol))
                .Where(x => x != null)
                .ToList();

            foreach (var op in operationDescriptors.Where(x => IsClassTopLevel(ctx, x!)))
            {
                ctx.AddSource($"{op!.ClassSymbol.Name}_partial.cs",
                    SourceText.From(GetOperationSource(op), Encoding.UTF8));
            }
        }

        private static string GetOperationSource(OperationDescriptor operationDescriptor)
        {
            var namespaceName = operationDescriptor.ClassSymbol.ContainingNamespace.ToDisplayString();
            var inputType = GetFinalInputType(operationDescriptor);
            var outputType = GetFinalOutputType(operationDescriptor);

            return $@"
namespace {namespaceName}
{{
{GetDtoSource(inputType, operationDescriptor.InputProperties, operationDescriptor.InputBaseInterface)}
{GetDtoSource(outputType, operationDescriptor.OutputProperties, operationDescriptor.OutputBaseInterface)}
    public partial class {operationDescriptor.ClassSymbol.Name} : {operationDescriptor.OperationType}<{inputType}, {outputType}>
    {{
    }}
}}
";
        }

        private static string GetFinalInputType(OperationDescriptor operationDescriptor)
        {
            if (string.IsNullOrWhiteSpace(operationDescriptor.InputProperties))
                return operationDescriptor.InputType ?? "Meteor.Operation.NoType";

            return operationDescriptor.InputClassName ?? $"{operationDescriptor.ClassSymbol.Name}InputDto";
        }

        private static string GetFinalOutputType(OperationDescriptor operationDescriptor)
        {
            if (string.IsNullOrWhiteSpace(operationDescriptor.OutputProperties))
                return operationDescriptor.OutputType ?? "Meteor.Operation.NoType";

            return operationDescriptor.OutputClassName ?? $"{operationDescriptor.ClassSymbol.Name}OutputDto";
        }

        private static bool IsClassTopLevel(GeneratorExecutionContext ctx, OperationDescriptor operationDescriptor)
        {
            if (operationDescriptor!.ClassSymbol!
                .ContainingSymbol.Equals(operationDescriptor.ClassSymbol.ContainingNamespace,
                    SymbolEqualityComparer.Default))
                return true;

            ctx.Report(Errors.MustBeTopLevel, operationDescriptor!.ClassSymbol!.Name);
            return false;
        }

        private static string? GetDtoSource(string className, string? properties, string? baseInterface)
        {
            if (string.IsNullOrWhiteSpace(properties))
                return null;

            baseInterface = string.IsNullOrWhiteSpace(baseInterface) ? null : $" : {baseInterface}";

            return $@"    public class {className}{baseInterface}
    {{
{string.Join(Environment.NewLine, properties!
                .Split(';', ',')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => $"        public {x.Trim()} {{ get; init; }}"))}
    }}";
        }

        private static OperationDescriptor? GetOperationDescriptor(GeneratorExecutionContext ctx,
            SyntaxNode classNode, ISymbol operationAttributeSymbol)
        {
            var classSymbol = GetClassSymbol(ctx, classNode);
            if (classSymbol == null)
                return null;

            var attributeData = GetOperationAttributeData(classSymbol, operationAttributeSymbol);
            if (attributeData == null)
                return null;

            var args = attributeData.ConstructorArguments.ToList();
            var operationType = attributeData.NamedArguments.FirstOrDefault(x => x.Key == "Type")
                .Value.Value?.ToString()?.Replace("<,>", "");

            return new OperationDescriptor(classSymbol)
            {
                OperationType = operationType ?? attributeData.AttributeClass?.ToDisplayString().Replace("Attribute", "") + "Async",
                InputType = args.Count > 0 && args[0].Type?.Name == "Type" ? args[0].Value?.ToString() : null,
                OutputType = args.Count > 1 && args[1].Type?.Name == "Type" ? args[1].Value?.ToString() : null,
                InputProperties = args.Count > 0 && args[0].Type?.Name == "String" ? args[0].Value?.ToString() : null,
                OutputProperties = args.Count > 1 && args[1].Type?.Name == "String" ? args[1].Value?.ToString() : null,
                InputClassName = attributeData.NamedArguments.FirstOrDefault(x => x.Key == "InputClassName").Value.Value
                    ?.ToString(),
                OutputClassName = attributeData.NamedArguments.FirstOrDefault(x => x.Key == "OutputClassName").Value
                    .Value?.ToString(),
                InputBaseInterface = attributeData.NamedArguments.FirstOrDefault(x => x.Key == "InputBaseInterface")
                    .Value.Value?.ToString(),
                OutputBaseInterface = attributeData.NamedArguments.FirstOrDefault(x => x.Key == "OutputBaseInterface")
                    .Value.Value?.ToString(),
            };
        }

        private static INamedTypeSymbol? GetClassSymbol(GeneratorExecutionContext context, SyntaxNode classNode) =>
            context.Compilation.GetSemanticModel(classNode.SyntaxTree)
                .GetDeclaredSymbol(classNode) as INamedTypeSymbol;

        private static AttributeData? GetOperationAttributeData(ISymbol classSymbol,
            ISymbol operationAttributeSymbol) =>
            classSymbol.GetAttributes().SingleOrDefault(x => 
                (x.AttributeClass?.Equals(operationAttributeSymbol, SymbolEqualityComparer.Default) ?? false)
                || (x.AttributeClass?.BaseType?.Equals(operationAttributeSymbol, SymbolEqualityComparer.Default) ?? false));
    }
}