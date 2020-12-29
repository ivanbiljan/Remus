using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Remus.Analyzers.Test {
    [TestClass]
    public sealed class OptionalArgumentAnalyzerTests : DiagnosticVerifier {
        [TestMethod]
        public void Test()
        {
            var source = @"
                            using System;
                            using System.Collections.Generic;
                            using System.Linq;
                            using System.Text;
                            using System.Threading.Tasks;
                            using System.Diagnostics;
                            namespace ConsoleApplication1
                            {
                                class Program
                                {
                                    [CommandHandler(""name"", """")]
                                    public void CommandHandler([OptionalArgument("""")] string parameter) {
                                    }
                                }
                            }";

            VerifyCSharpDiagnostic(source, new DiagnosticResult());
        }
    }
}
