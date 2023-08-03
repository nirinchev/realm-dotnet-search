using System;
using System.IO;
using MongoDB.Bson;
using Realms.Search;
using static Realms.ThreadSafeReference;

namespace Realms.Search;

/// <summary>
/// The base interface for all search definitions
/// </summary>
public interface ISearchDefinition
{
    /// <summary>
    /// Render the search definition as a bson document.
    /// </summary>
    /// <returns>The document that represents the definition.</returns>
    BsonDocument Render();
}

/// <summary>
/// The base class for all query search definitions.
/// </summary>
public abstract class SearchDefinitionBase : ISearchDefinition
{
    /// <summary>
    /// The name of the operator this search definition represents
    /// </summary>
    protected abstract string OperatorName { get; }

    /// <summary>
    /// Indexed <see href="https://www.mongodb.com/docs/atlas/atlas-search/define-field-mappings/#std-label-bson-data-types-autocomplete">autocomplete</see>
    /// type of field to search.
    /// </summary>
    /// <value>The field path in the document.</value>
    public PathDefinition Path { get; }

    /// <summary>
    /// <see href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/#std-label-scoring-ref">The score</see> assigned to matching search term results
    /// </summary>
    /// <value>The score assigned to search matches.</value>
    public ScoreOptions? Score { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchDefinitionBase"/> class..
    /// </summary>
    /// <param name="path">The field path in the document.</param>
    /// <param name="score">The score assigned to search matches.</param>
    protected SearchDefinitionBase(PathDefinition path, ScoreOptions? score)
    {
        Score = score;
        Path = path;
    }

    BsonDocument ISearchDefinition.Render()
    {
        var definition = new BsonDocument
        {
            ["path"] = Path.Value
        };

        if (Score != null)
        {
            definition["score"] = Score.Render();
        }

        PopulateDefinition(definition);

        return new(OperatorName, definition);
    }

    /// <summary>
    /// Populate the definition with any fields that are added in the derived class.
    /// </summary>
    /// <param name="baseDefinition">The definition populated with the <see cref="Path"/> and the <see cref="Score"/>.</param>
    protected abstract void PopulateDefinition(BsonDocument baseDefinition);
}

/// <summary>
/// A base class for all search definitions that have a query, path, and score options.
/// </summary>
public abstract class QuerySearchDefinitionBase : SearchDefinitionBase
{

    /// <summary>
    /// String or strings to search for. If there are multiple terms in a string, Atlas Search also looks for a match for each term in the string separately.
    /// </summary>
    /// <value>The search query.</value>
    public QueryDefinition Query { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuerySearchDefinitionBase"/> class..
    /// </summary>
    /// <param name="query">The string to search for.</param>
    /// <param name="path">The field path in the document.</param>
    /// <param name="score">The score assigned to search matches.</param>
    protected QuerySearchDefinitionBase(QueryDefinition query, PathDefinition path, ScoreOptions? score)
        : base(path, score)
    {
        Query = query;
    }

    /// <inheritdoc/>
    protected override void PopulateDefinition(BsonDocument baseDefinition)
    {
        baseDefinition["query"] = Query.Value;
    }
}

/// <summary>
/// Base class for all search definitions that are capable of executing fuzzy searches.
/// </summary>
public abstract class FuzzySearchDefinitionBase : QuerySearchDefinitionBase
{
    /// <summary>
    /// Enable fuzzy search. Find strings which are similar to the search term or terms.
    /// </summary>
    /// <value>The fuzzy search options.</value>
    public FuzzyOptions? Fuzzy { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FuzzySearchDefinitionBase"/> class..
    /// </summary>
    /// <param name="query">The string to search for.</param>
    /// <param name="path">The field path in the document.</param>
    /// <param name="score">The score assigned to search matches.</param>
    /// <param name="fuzzy">The fuzzy search options.</param>
    protected FuzzySearchDefinitionBase(QueryDefinition query, PathDefinition path, ScoreOptions? score, FuzzyOptions? fuzzy)
        : base(query, path, score)
    {
        Fuzzy = fuzzy;
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

