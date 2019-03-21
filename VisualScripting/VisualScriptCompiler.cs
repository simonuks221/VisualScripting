using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace VisualScripting
{
    class VisualScriptCompiler
    {
        public VisualScriptCompiler(string _codeToCompile, ProjectManager _projectManager)
        {
            ConsoleForm.Instance.AddNewMessage("Started compiling");
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(_codeToCompile);
            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ConsoleForm).Assembly.Location),
             MetadataReference.CreateFromFile(typeof(Form).Assembly.Location),
             MetadataReference.CreateFromFile(typeof(IComponent).Assembly.Location)

            };
            
            CSharpCompilation compilation = CSharpCompilation.Create(
        assemblyName,
        syntaxTrees: new[] { syntaxTree },
        references: references,
        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        ConsoleForm.Instance.AddNewMessage(diagnostic.Id + diagnostic.GetMessage());
                    }
                    Console.Out.WriteLine("/n" +_codeToCompile);
                }
                else
                {
                    ConsoleForm.Instance.AddNewMessage("End compiling");
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Type type = assembly.GetType("VisualScripting.Program");
                    object obj = Activator.CreateInstance(type);
                    
                    type.InvokeMember("Constructor", //Invoke constructor
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { }// "Hello World" }
                        );
                }
            }
            references = null;
            syntaxTree = null;
            assemblyName = null;
            compilation = null;
        }
    }
}
