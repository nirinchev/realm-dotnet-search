using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Realm.Search.SourceGenerator;

internal class SyntaxContextReceiver : ISyntaxContextReceiver
{
    private readonly Dictionary<ITypeSymbol, SearchClassDefinition> _realmClassesDict = new(SymbolEqualityComparer.Default);

    public IReadOnlyCollection<SearchClassDefinition> SearchClasses => _realmClassesDict.Values;

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classSyntax)
        {
            return;
        }

        var classSymbol = (ITypeSymbol)context.SemanticModel.GetDeclaredSymbol(classSyntax)!;

        if (_realmClassesDict.TryGetValue(classSymbol, out var rcDefinition))
        {
            rcDefinition.ClassDeclarations.Add(classSyntax);
            return;
        }

        if (classSymbol.Interfaces.Any(i => i.Name == "ISearchModel"))
        {
            var realmClassDefinition = new SearchClassDefinition(classSymbol, new List<ClassDeclarationSyntax> { classSyntax });
            _realmClassesDict.Add(classSymbol, realmClassDefinition);
        }
    }
}

internal struct SearchClassDefinition
{
    public ITypeSymbol ClassSymbol { get; }

    public List<ClassDeclarationSyntax> ClassDeclarations { get; }

    public SearchClassDefinition(ITypeSymbol classSymbol, List<ClassDeclarationSyntax> classDeclarations)
    {
        ClassSymbol = classSymbol;
        ClassDeclarations = classDeclarations;
    }
}

