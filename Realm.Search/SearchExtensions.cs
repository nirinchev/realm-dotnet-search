using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MongoDB.Bson;
using Realm.Search;
using Realms.Search;

namespace Realms.Sync;

public static class SearchExtensions
{
    private static readonly Dictionary<Type, ProjectionModel?> _defaultProjections = new();

    public static Task Search<T>(this MongoClient.Collection<T> collection)
        where T : class
    {
        throw new NotImplementedException();
    }

    public static Task<TModel[]> Autocomplete<TModel>(
        this MongoClient.Collection<TModel> collection,
        QueryDefinition query,
        string path,
        ProjectionModel? projection = null,
        HighlightOptions? highlightOptions = null,
        int limit = 20)
            where TModel : class, ISearchModel
    {
        var autocompleteDefinition = new BsonDocument
        {
            ["query"] = query.Value,
            ["path"] = path,
        };

        var autocompleteStage = new BsonDocument("autocomplete", autocompleteDefinition);
        return collection.SearchCore<TModel>(autocompleteStage, projection, highlightOptions, limit);
    }

    private static Task<TModel[]> SearchCore<TModel>(this MongoClient.Collection<TModel> collection, BsonDocument searchStage, ProjectionModel? projection, HighlightOptions? highlightOptions, int? limit)
        where TModel : class, ISearchModel
    {
        var projectStage = GetProjection<TModel>(projection);

        if (highlightOptions != null)
        {
            // If we have highlight options, we need to include them in the search stage
            searchStage["highlight"] = highlightOptions.Render();

            // If we're projecting, we need to also include the highlights in the project stage 
            if (projectStage != null && !projectStage.Contains("searchHighlights"))
            {
                projectStage["searchHighlights"] = new BsonDocument("$meta", "searchHighlights");
            }
        }

        var stages = new List<object>
        {
            new BsonDocument("$search", searchStage)
        };

        if (limit != null)
        {
            stages.Add(new BsonDocument("$limit", limit.Value));
        }

        if (projectStage != null)
        {
            stages.Add(new BsonDocument("$project", projectStage));
        }

        return collection.AggregateAsync<TModel>(stages.ToArray());
    }

    private static BsonDocument? GetProjection<TModel>(ProjectionModel? projection)
        where TModel : class, ISearchModel
    {
        if (projection != null)
        {
            return projection.Render();
        }

        var modelType = typeof(TModel);
        if (!_defaultProjections.TryGetValue(modelType, out var value))
        {
            value = modelType.GetProperty("DefaultProjection", BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as ProjectionModel;
            _defaultProjections[modelType] = value;
        }

        return value?.Render();
    }
}

