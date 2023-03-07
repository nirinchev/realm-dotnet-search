using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Realm.Search.SourceGenerator;

internal class DiagnosticsEmitter
{
    private readonly GeneratorExecutionContext _context;

    public DiagnosticsEmitter(GeneratorExecutionContext context)
    {
        _context = context;
    }

    public void Emit(ParsingResults parsingResults)
    {
        foreach (var classInfo in parsingResults.ClassInfo)
        {
            if (!classInfo.Diagnostics.Any())
            {
                continue;
            }

            try
            {
                classInfo.Diagnostics.ForEach(_context.ReportDiagnostic);
            }
            catch (Exception ex)
            {
                _context.ReportDiagnostic(Diagnostics.UnexpectedError(classInfo.Name, ex.Message, ex.StackTrace));
                throw;
            }
        }
    }
}