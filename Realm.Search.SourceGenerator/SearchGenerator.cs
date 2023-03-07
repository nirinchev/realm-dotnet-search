using System;
using Microsoft.CodeAnalysis;

namespace Realm.Search.SourceGenerator;

[Generator]
public class SearchGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is not SyntaxContextReceiver scr)
        {
            return;
        }

        var parser = new Parser(context);
        var parsingResults = parser.Parse(scr.SearchClasses);

        var diagnosticsEmitter = new DiagnosticsEmitter(context);
        diagnosticsEmitter.Emit(parsingResults);

        var codeEmitter = new CodeEmitter(context);
        codeEmitter.Emit(parsingResults);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxContextReceiver());
    }
}

