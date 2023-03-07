using System;
namespace Realm.Search;

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
}

