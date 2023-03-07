using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Realm.Search.SourceGenerator;

internal record ClassInfo
{
    public string Name { get; set; } = null!;

    public NamespaceInfo NamespaceInfo { get; set; } = null!;

    public Accessibility Accessibility { get; set; }

    public ITypeSymbol TypeSymbol { get; set; } = null!;

    public List<PropertyInfo> Properties { get; } = new();

    public List<Diagnostic> Diagnostics { get; } = new();

    public List<string> Usings { get; } = new();

    public List<EnclosingClassInfo> EnclosingClasses { get; } = new();

    public bool HasDuplicatedName { get; set; }
}

internal record NamespaceInfo
{
    public string? OriginalName { get; }

    [MemberNotNullWhen(returnValue: false, member: nameof(OriginalName))]
    public bool IsGlobal { get; }

    public static NamespaceInfo Global() => new(originalName: null, isGlobal: true);

    public static NamespaceInfo Local(string originalName) => new(originalName: originalName, isGlobal: false);

    private NamespaceInfo(string? originalName, bool isGlobal)
    {
        OriginalName = originalName;
        IsGlobal = isGlobal;
    }

    public string ComputedName => IsGlobal ? "Global" : OriginalName;
}

internal record EnclosingClassInfo(string Name, Accessibility Accessibility);

internal record PropertyInfo(string Name)
{
    public Accessibility Accessibility { get; set; }
}