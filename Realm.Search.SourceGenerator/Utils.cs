using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Realm.Search.SourceGenerator;

internal static class Utils
{
    public static Location GetIdentifierLocation(this ClassDeclarationSyntax cds)
    {
        // If we return the location on the ClassDeclarationSyntax, then the whole class will be selected.
        // "Identifier" points only to the class name
        return cds.Identifier.GetLocation();
    }

    public static NamespaceInfo GetNamespaceInfo(this ITypeSymbol classSymbol)
    {
        if (classSymbol.ContainingNamespace.IsGlobalNamespace)
        {
            return NamespaceInfo.Global();
        }

        return NamespaceInfo.Local(classSymbol.ContainingNamespace.ToDisplayString());
    }

    public static bool HasAttribute(this ISymbol symbol, string attributeName)
    {
        return symbol.GetAttributes().Any(a => a.AttributeClass?.Name == attributeName);
    }

    public static bool IsAutomaticProperty(this PropertyDeclarationSyntax propertySyntax)
    {
        // This means the property has explicit getter and/or setter
        if (propertySyntax.AccessorList != null)
        {
            // Body is "classic" curly brace body
            // ExpressionBody is =>
            return !propertySyntax.AccessorList.Accessors.Any(a => a.Body != null || a.ExpressionBody != null);
        }

        // This means the body is => (propertySyntax.ExpressionBody != null)
        return false;
    }

    public static string Indent(this string str, int indents = 1, bool trimNewLines = false)
    {
        var indentString = new string(' ', indents * 4);

        var sb = new StringBuilder();
        var lines = str.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                sb.Append(indentString);
            }

            sb.AppendLine(line);
        }

        sb.Remove(sb.Length - Environment.NewLine.Length, Environment.NewLine.Length);

        var result = sb.ToString();
        if (trimNewLines)
        {
            result = result.TrimEnd();
        }

        return result;
    }
}
