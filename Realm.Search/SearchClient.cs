using MongoDB.Bson;
using Realm.Search;
using Realms.Sync;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Realms.Search;

public readonly struct SearchClient<TModel>
    where TModel : class, ISearchModel
{
    private readonly MongoClient.Collection<TModel> _collection;
    private readonly string? _index;

    internal SearchClient(MongoClient.Collection<TModel> collection, string? index)
    {
        _collection = collection;
        _index = index;
    }

    public Task<TModel[]> Autocomplete(
        AutocompleteDefinition autocomplete,
        ProjectionModel? projection = null,
        HighlightOptions? highlightOptions = null,
        int limit = 20)
    {
        return SearchCore(autocomplete, projection, highlightOptions, limit);
    }

    public Task<TModel[]> Compound(
        CompoundDefinition compound,
        ProjectionModel? projection = null,
        HighlightOptions? highlightOptions = null,
        int limit = 20)
    {
        return SearchCore(compound, projection, highlightOptions, limit);
    }

    private Task<TModel[]> SearchCore(ISearchDefinition searchDefinition, ProjectionModel? projection, HighlightOptions? highlightOptions, int? limit)
    {
        var projectStage = ProjectionHelper.GetProjection<TModel>(projection);

        var searchStage = searchDefinition.Render();

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

        if (_index != null)
        {
            searchStage["index"] = _index;
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

        return _collection.AggregateAsync<TModel>(stages.ToArray());
    }
}

