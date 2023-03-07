using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Realm.Search.SourceGenerator;

internal class CodeEmitter
{
    private GeneratorExecutionContext _context;

    public CodeEmitter(GeneratorExecutionContext context)
    {
        _context = context;
    }

    public void Emit(ParsingResults parsingResults)
    {
        foreach (var classInfo in parsingResults.ClassInfo)
        {
            if (!ShouldEmit(classInfo))
            {
                continue;
            }

            try
            {
                var generatedSource = new ClassCodeBuilder(classInfo).GenerateSource();

                // Replace all occurrences of at least 3 newlines with only 2
                var formattedSource = Regex.Replace(generatedSource, @$"[{Environment.NewLine}]{{3,}}", $"{Environment.NewLine}{Environment.NewLine}");

                var sourceText = SourceText.From(formattedSource, Encoding.UTF8);

                // Discussion on allowing duplicate hint names: https://github.com/dotnet/roslyn/discussions/60272
                var className = classInfo.HasDuplicatedName ? $"{classInfo.NamespaceInfo.ComputedName}_{classInfo.Name}" : classInfo.Name;

                _context.AddSource($"{className}.g.cs", sourceText);
            }
            catch (Exception ex)
            {
                _context.ReportDiagnostic(Diagnostics.UnexpectedError(classInfo.Name, ex.Message, ex.StackTrace));
                throw;
            }
        }
    }

    private static bool ShouldEmit(ClassInfo classInfo)
    {
        return !classInfo.Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);
    }
}