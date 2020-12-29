using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Remus.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class OptionalArgumentAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = "Remus0001";

        private const string Category = "CodeFix";

        private static readonly LocalizableString Title = new LocalizableResourceString(
            nameof(Resources.OptionalArgumentAnalyzerTitle), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(
            nameof(Resources.OptionalArgumentAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString Description = new LocalizableResourceString(
            nameof(Resources.OptionalArgumentAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
        }

        private void AnalyzeMethod(SymbolAnalysisContext context)
        {
            if (context.Symbol is not IMethodSymbol methodSymbol)
            {
                return;
            }

            var commandHandlerAttribute =
                context.Compilation.GetTypeByMetadataName("Remus.Attributes.CommandHandlerAttribute");
            var methodAttributes = methodSymbol.GetAttributes();
            if (!methodAttributes.Any(ad => SymbolEqualityComparer.Default.Equals(ad.AttributeClass, commandHandlerAttribute)))
            {
                return;
            }

            var optionalArgumentAttribute =
                context.Compilation.GetTypeByMetadataName("Remus.Attributes.OptionalArgumentAttribute");
            foreach (var parameterSymbol in methodSymbol.Parameters)
            {
                var parameterAttributes = parameterSymbol.GetAttributes();
                var hasOptionalArgumentAttribute = parameterAttributes.Any(ad =>
                    SymbolEqualityComparer.Default.Equals(ad.AttributeClass, optionalArgumentAttribute));
                if (hasOptionalArgumentAttribute && !parameterSymbol.IsOptional || parameterSymbol.IsOptional && !hasOptionalArgumentAttribute)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, parameterSymbol.Locations[0]));
                }
            }
        }
    }
}