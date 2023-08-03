using System;
namespace Realms.Search;

internal static class Argument
{
    public static T NotNull<T>(T? value, string paramName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(paramName);
        }

        return value;
    }

    public static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new ArgumentException(message);
        }
    }
}

