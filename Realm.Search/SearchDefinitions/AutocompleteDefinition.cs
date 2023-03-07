using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Realms.Search;

namespace Realm.Search
{
    public class AutocompleteDefinition : ISearchDefinition
    {
        public QueryDefinition Query { get; }

        public string Path { get; }

        public TokenOrder TokenOrder { get; }

        public FuzzyOptions? Fuzzy { get; }

        public AutocompleteScoreOptions? Score { get; }

        public AutocompleteDefinition(QueryDefinition query, string path, TokenOrder tokenOrder = TokenOrder.Any, FuzzyOptions? fuzzy = null, AutocompleteScoreOptions? score = null)
        {
            Query = query;
            Path = path;
            TokenOrder = tokenOrder;
            Fuzzy = fuzzy;
            Score = score;
        }

        BsonDocument ISearchDefinition.Render()
        {
            var autocompleteDefinition = new BsonDocument
            {
                ["query"] = Query.Value,
                ["path"] = Path,
            };

            if (TokenOrder != TokenOrder.Any)
            {
                autocompleteDefinition["tokenOrder"] = TokenOrder.ToString().ToLowerInvariant();
            }

            if (Fuzzy != null)
            {
                autocompleteDefinition["fuzzy"] = Fuzzy.Render();
            }

            if (Score != null)
            {
                autocompleteDefinition["score"] = Score.Render();
            }

            return new BsonDocument("autocomplete", autocompleteDefinition);
        }
    }

    public enum TokenOrder
    {
        Any,
        Sequential
    }
}

