using System;
using MongoDB.Bson;
using Realms.Search;
using System.Reflection;
using System.Collections.Concurrent;

namespace Realms.Search;

internal static class ProjectionHelper
{
    private static readonly ConcurrentDictionary<Type, ProjectionModel?> _defaultProjections = new();

    public static BsonDocument? GetProjection<TModel>(ProjectionModel? projection)
        where TModel : class, ISearchModel
    {
        if (projection != null)
        {
            return projection.Render();
        }

        var modelType = typeof(TModel);
        var value = _defaultProjections.GetOrAdd(modelType, t => t.GetProperty("DefaultProjection", BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as ProjectionModel);
        return value?.Render();
    }
}
