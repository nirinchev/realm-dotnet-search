using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Realms.Search;

namespace Realm.Search
{
    /// <summary>
    /// A search definition for an <see href="https://www.mongodb.com/docs/atlas/atlas-search/autocomplete/">autocomplete</see> search.
    /// </summary>
    public class AutocompleteDefinition : ISearchDefinition
    {
        /// <summary>
        /// String or strings to search for. If there are multiple terms in a string, Atlas Search also looks for a match for each term in the string separately.
        /// </summary>
        /// <value>The search query.</value>
        public QueryDefinition Query { get; }

        /// <summary>
        /// Indexed <see href="https://www.mongodb.com/docs/atlas/atlas-search/define-field-mappings/#std-label-bson-data-types-autocomplete">autocomplete</see>
        /// type of field to search.
        /// </summary>
        /// <value>The field path in the document.</value>
        public string Path { get; }

        /// <summary>
        /// Order in which to search for tokens.
        /// </summary>
        /// <value>The search token order.</value>
        public TokenOrder TokenOrder { get; }

        /// <summary>
        /// Enable fuzzy search. Find strings which are similar to the search term or terms.
        /// </summary>
        /// <value>The fuzzy search options.</value>
        public FuzzyOptions? Fuzzy { get; }

        /// <summary>
        /// <see href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/#std-label-scoring-ref">The score</see> assigned to matching search term results
        /// </summary>
        /// <value>The score assigned to search matches.</value>
        public AutocompleteScoreOptions? Score { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutocompleteDefinition"/> class.
        /// </summary>
        /// <param name="query">The string to search for.</param>
        /// <param name="path">The field path in the document.</param>
        /// <param name="tokenOrder">The search token order.</param>
        /// <param name="fuzzy">The fuzzy search options.</param>
        /// <param name="score">The score assigned to search matches.</param>
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

    /// <summary>
    /// The order of tokens to search for.
    /// </summary>
    public enum TokenOrder
    {
        /// <summary>
        /// Indicates tokens in the query can appear in any order in the documents. Results contain
        /// documents where the tokens appear sequentially and non-sequentially. However, results
        /// where the tokens appear sequentially score higher than other, non-sequential values.
        /// </summary>
        Any,

        /// <summary>
        /// Indicates tokens in the query must appear adjacent to each other or in the order specified
        /// in the query in the documents. Results contain only documents where the tokens appear sequentially.
        /// </summary>
        Sequential
    }
}

