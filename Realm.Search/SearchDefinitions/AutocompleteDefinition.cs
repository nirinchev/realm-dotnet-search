using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Realms.Search;

namespace Realms.Search;

/// <summary>
/// A search definition for an <see href="https://www.mongodb.com/docs/atlas/atlas-search/autocomplete/">autocomplete</see> search.
/// </summary>
public class AutocompleteDefinition : FuzzySearchDefinitionBase
{
    /// <inheritdoc/>
    protected override string OperatorName => "autocomplete";

    /// <summary>
    /// Order in which to search for tokens.
    /// </summary>
    /// <value>The search token order.</value>
    public TokenOrder TokenOrder { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutocompleteDefinition"/> class.
    /// </summary>
    /// <param name="query">The string to search for.</param>
    /// <param name="path">The field path in the document.</param>
    /// <param name="tokenOrder">The search token order.</param>
    /// <param name="fuzzy">The fuzzy search options.</param>
    /// <param name="score">The score assigned to search matches.</param>
    public AutocompleteDefinition(QueryDefinition query, PathDefinition path, TokenOrder tokenOrder = TokenOrder.Any, FuzzyOptions? fuzzy = null, ScoreOptions? score = null)
        : base(query, path, score, fuzzy)
    {
        TokenOrder = tokenOrder;
    }

    /// <inheritdoc/>
    protected override void PopulateDefinition(BsonDocument baseDefinition)
    {
        base.PopulateDefinition(baseDefinition);

        if (Fuzzy != null)
        {
            baseDefinition["fuzzy"] = Fuzzy.Render();
        }
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

