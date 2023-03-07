using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Realm.Search.SourceGenerator;

internal class Parser
{
    private readonly GeneratorExecutionContext _context;

    public Parser(GeneratorExecutionContext context)
    {
        _context = context;
    }

    public ParsingResults Parse(IEnumerable<SearchClassDefinition> realmClasses)
    {
        var result = new ParsingResults();
        var classNames = new HashSet<string>();
        var duplicateClassNames = new HashSet<string>();

        foreach (var rc in realmClasses)
        {
            var classSymbol = rc.ClassSymbol;
            var classDeclarations = rc.ClassDeclarations;

            var classInfo = new ClassInfo();

            // We tie the diagnostics to the first class declaration only.
            var firstClassDeclarationSyntax = classDeclarations.First();

            try
            {
                if (!firstClassDeclarationSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
                {
                    classInfo.Diagnostics.Add(Diagnostics.ClassNotPartial(classSymbol.Name, firstClassDeclarationSyntax.GetIdentifierLocation()));
                }

                classInfo.Name = classSymbol.Name;
                classInfo.NamespaceInfo = classSymbol.GetNamespaceInfo();
                classInfo.Accessibility = classSymbol.DeclaredAccessibility;
                classInfo.TypeSymbol = classSymbol;
                classInfo.EnclosingClasses.AddRange(GetEnclosingClassList(classInfo, classSymbol, firstClassDeclarationSyntax));

                if (!classNames.Add(classInfo.Name))
                {
                    duplicateClassNames.Add(classInfo.Name);
                }

                // Properties
                foreach (var classDeclarationSyntax in classDeclarations)
                {
                    var semanticModel = _context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
                    var propertiesSyntax = classDeclarationSyntax.ChildNodes().OfType<PropertyDeclarationSyntax>();

                    classInfo.Usings.AddRange(GetUsings(classDeclarationSyntax));
                    classInfo.Properties.AddRange(GetProperties(propertiesSyntax, semanticModel));
                }

                result.ClassInfo.Add(classInfo);
            }
            catch (Exception ex)
            {
                classInfo.Diagnostics.Add(Diagnostics.UnexpectedError(classSymbol.Name, ex.Message, ex.StackTrace));
                throw;
            }
        }

        foreach (var classInfo in result.ClassInfo.Where(c => duplicateClassNames.Contains(c.Name)))
        {
            classInfo.HasDuplicatedName = true;
        }

        return result;
    }

    private IEnumerable<PropertyInfo> GetProperties(IEnumerable<PropertyDeclarationSyntax> propertyDeclarationSyntaxes, SemanticModel model)
    {
        foreach (var propSyntax in propertyDeclarationSyntaxes)
        {
            var propSymbol = model.GetDeclaredSymbol(propSyntax)!;

            if (propSymbol.HasAttribute("BsonIgnoreAttribute") || propSymbol.IsStatic || !propSyntax.IsAutomaticProperty())
            {
                continue;
            }

            var info = new PropertyInfo(propSymbol.Name)
            {
                MapTo = propSymbol.GetAttributeArgument("BsonElementAttribute") as string,
            };

            yield return info;
        }
    }

    private static IList<EnclosingClassInfo> GetEnclosingClassList(ClassInfo classInfo, ITypeSymbol classSymbol, ClassDeclarationSyntax classDeclarationSyntax)
    {
        var enclosingClassList = new List<EnclosingClassInfo>();
        var currentSymbol = classSymbol;
        var currentClassDeclaration = classDeclarationSyntax;

        while (currentSymbol.ContainingSymbol is ITypeSymbol ts && currentClassDeclaration.Parent is ClassDeclarationSyntax cs)
        {
            if (!cs.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
            {
                classInfo.Diagnostics.Add(Diagnostics.ParentOfNestedClassIsNotPartial(classSymbol.Name, ts.Name, cs.GetIdentifierLocation()));
            }

            var enclosingClassinfo = new EnclosingClassInfo(ts.Name, ts.DeclaredAccessibility);
            enclosingClassList.Add(enclosingClassinfo);

            currentSymbol = ts;
            currentClassDeclaration = cs;
        }

        return enclosingClassList;
    }

    private static IEnumerable<string> GetUsings(ClassDeclarationSyntax classDeclarationSyntax)
    {
        var usings = new List<string>();

        var compilationUnitSyntax = classDeclarationSyntax.FirstAncestorOrSelf<CompilationUnitSyntax>();

        if (compilationUnitSyntax != null)
        {
            var usingDirectives = compilationUnitSyntax.ChildNodes()
                .Where(c => c.IsKind(SyntaxKind.UsingDirective))
                .OfType<UsingDirectiveSyntax>()
                .Select(RemoveUsingKeyword);
            usings.AddRange(usingDirectives);
        }

        return usings;
    }

    private static string RemoveUsingKeyword(UsingDirectiveSyntax syntax)
    {
        var components = new object?[] { syntax.StaticKeyword, syntax.Alias, syntax.Name }
            .Select(o => o?.ToString())
            .Where(o => !string.IsNullOrEmpty(o));
        return string.Join(" ", components);
    }
}

internal record ParsingResults
{
    public List<ClassInfo> ClassInfo { get; } = new();
}