using System;
using Microsoft.CodeAnalysis;

namespace Realm.Search.SourceGenerator;

internal static class Diagnostics
{
    private enum Id
    {
        UnexpectedError = 1,
        ClassNotPartial = 2,
        ParentOfNestedClassIsNotPartial = 3,
    }

    #region Errors

    public static Diagnostic UnexpectedError(string className, string message, string stackTrace)
    {
        return CreateDiagnosticError(
            Id.UnexpectedError,
            "Unexpected error during source generation",
            $"There was an unexpected error during source generation of class {className}",
            Location.None,
            description: $"Exception Message: {message}. \r\nCallstack:\r\n{stackTrace}");
    }

    public static Diagnostic ClassNotPartial(string className, Location location)
    {
        return CreateDiagnosticError(
            Id.ClassNotPartial,
            "Realm classes need to be defined as partial",
            $"Class {className} is a Realm class but it is not declared as partial",
            location);
    }

    public static Diagnostic ParentOfNestedClassIsNotPartial(string className, string parentClassName, Location location)
    {
        return CreateDiagnosticError(
            Id.ParentOfNestedClassIsNotPartial,
            "Containing class of nested Realm class is not declared as partial",
            $"Class {parentClassName} contains nested Realm class {className} and needs to be declared as partial.",
            location);
    }

    #endregion

    private static Diagnostic CreateDiagnostic(Id id, string title, string messageFormat, DiagnosticSeverity severity,
        Location location, string category, string? description)
    {
        var reportedId = $"RLM{(int)id:000}";
        DiagnosticDescriptor descriptor = new(reportedId, title, messageFormat, category, severity, isEnabledByDefault: true, description: description);

        return Diagnostic.Create(descriptor, location);
    }

    private static Diagnostic CreateDiagnosticError(Id id, string title, string messageFormat,
        Location location, string category = "RealmClassGeneration", string? description = null)
        => CreateDiagnostic(id, title, messageFormat, DiagnosticSeverity.Error, location, category, description);

    private static Diagnostic CreateDiagnosticWarning(Id id, string title, string messageFormat,
        Location location, string category = "RealmClassGeneration", string? description = null)
        => CreateDiagnostic(id, title, messageFormat, DiagnosticSeverity.Warning, location, category, description);
}

