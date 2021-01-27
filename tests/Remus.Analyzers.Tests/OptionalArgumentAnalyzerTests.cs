using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Remus.Analyzers.Tests.Helpers;
using Xunit;

namespace Remus.Analyzers.Tests
{
    public sealed class OptionalArgumentAnalyzerTests : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new OptionalArgumentAnalyzer();
        }

        [Fact]
        public void MethodDoesNotHaveCommandHandlerAttribute_NoDiagnosticIsTriggered()
        {
            const string source = @"
using System;
namespace AnalyzerTest {
    class Program {
        static void Main(string[] args) {
            
        }

        private static void CommandHandlerMethod() {
        }
    }
}";

            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void OptionalParameterDoesNotHaveOptionalArgumentAttribute_DiagnosticIsTriggered()
        {
            const string source = @"
using System;
namespace AnalyzerTest {
    class Program {
        static void Main(string[] args) {
        }

        [Remus.Attributes.CommandHandler(""name"", """")]
        private static void CommandHandlerMethod(int x = 0) {
        }
    }
}";

            var expectedDiagnosticResult = new DiagnosticResult
            {
                Id = "Remus0001",
                Message = new LocalizableResourceString(nameof(Resources.OptionalArgumentAnalyzerMessageFormat),
                    Resources.ResourceManager, typeof(Resources)).ToString(),
                Severity = DiagnosticSeverity.Error,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 9, 54)
                }
            };

            VerifyCSharpDiagnostic(source, expectedDiagnosticResult);
        }

        [Fact]
        public void ValidCode_NoDiagnosticIsTriggered()
        {
            const string source = @"
using System;
namespace AnalyzerTest {
    class Program {
        [Remus.Attributes.CommandHandler(""name"", """")]
        private static void CommandHandlerMethod([Remus.Attributes.OptionalArgument] int x = 0) {
        }
    }
}";

            VerifyCSharpDiagnostic(source);
        }
    }
}