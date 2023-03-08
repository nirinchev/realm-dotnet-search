using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Realms.Sync;
using Remotion.Linq.Clauses;

namespace Realms.Search;

/// <summary>
/// A search definition for an <see href="https://www.mongodb.com/docs/atlas/atlas-search/compound/">compound</see> search.
/// </summary>
public class CompoundDefinition : ISearchDefinition
{
	private List<BsonDocument> _mustClauses = new();
    private List<BsonDocument> _shouldClauses = new();
    private List<BsonDocument> _mustNotClauses = new();
    private List<BsonDocument> _filterClauses = new();

    /// <summary>
    /// Adds a clause that must match for the document to be included in the results. The returned score is the sum
    /// of the scores of all the subqueries in the clause.
    /// </summary>
    /// <remarks>Maps to the AND boolean operator.</remarks>
    /// <param name="document">The document representing the clause.</param>
    /// <returns>The same compound definition to allow for fluent chaining.</returns>
    public CompoundDefinition Must(BsonDocument document)
    {
        _mustClauses.Add(document);
        return this;
    }

    /// <summary>
    /// Adds a clause that must match for the document to be included in the results. The returned score is the sum
    /// of the scores of all the subqueries in the clause.
    /// </summary>
    /// <remarks>Maps to the AND boolean operator.</remarks>
    /// <param name="definition">The document representing the clause.</param>
    /// <returns>The same compound definition to allow for fluent chaining.</returns>
    public CompoundDefinition Must(ISearchDefinition definition) => Must(definition.Render());

    /// <summary>
    /// Adds a clause that should match in documents that are included in the results. Documents that contain a
    /// match for a should clause have higher scores than documents that don't contain a should clause. The returned
    /// score is the sum of the scores of all the subqueries in the clause. If you use more than one should clause,
    /// you can use the <see cref="MinimumShouldMatch"/> option to specify a minimum number of should clauses that must match to
    /// include a document in the results.
    /// </summary>
    /// <remarks>Maps to the OR boolean operator.</remarks>
    /// <param name="document">The document representing the clause.</param>
    /// <returns>The same compound definition to allow for fluent chaining.</returns>
    public CompoundDefinition Should(BsonDocument document)
    {
        _shouldClauses.Add(document);
        return this;
    }

    /// <summary>
    /// Adds a clause that should match in documents that are included in the results. Documents that contain a
    /// match for a should clause have higher scores than documents that don't contain a should clause. The returned
    /// score is the sum of the scores of all the subqueries in the clause. If you use more than one should clause,
    /// you can use the <see cref="MinimumShouldMatch"/> option to specify a minimum number of should clauses that must match to
    /// include a document in the results.
    /// </summary>
    /// <remarks>Maps to the OR boolean operator.</remarks>
    /// <param name="definition">The document representing the clause.</param>
    /// <returns>The same compound definition to allow for fluent chaining.</returns>
    public CompoundDefinition Should(ISearchDefinition definition) => Should(definition.Render());

    /// <summary>
    /// Adds a clause that must not match for a document to be included in the results. These clauses don't contribute to a returned document's score.
    /// </summary>
    /// <remarks>Maps to the AND NOT boolean operator.</remarks>
    /// <param name="document">The document representing the clause.</param>
    /// <returns>The same compound definition to allow for fluent chaining.</returns>
    public CompoundDefinition MustNot(BsonDocument document)
    {
        _mustNotClauses.Add(document);
        return this;
    }

    /// <summary>
    /// Adds a clause that must not match for a document to be included in the results. These clauses don't contribute to a returned document's score.
    /// </summary>
    /// <remarks>Maps to the AND NOT boolean operator.</remarks>
    /// <param name="definition">The document representing the clause.</param>
    /// <returns>The same compound definition to allow for fluent chaining.</returns>
    public CompoundDefinition MustNot(ISearchDefinition definition) => MustNot(definition.Render());

    /// <summary>
    /// Adds a clause that must match for a document to be included in the results. These clauses do not contribute to a returned document's score.
    /// </summary>
    /// <param name="document">The document representing the clause.</param>
    /// <returns>The same compound definition to allow for fluent chaining.</returns>
    public CompoundDefinition Filter(BsonDocument document)
    {
        _filterClauses.Add(document);
        return this;
    }

    /// <summary>
    /// Adds a clause that must match for a document to be included in the results. These clauses do not contribute to a returned document's score.
    /// </summary>
    /// <param name="definition">The document representing the clause.</param>
    /// <returns>The same compound definition to allow for fluent chaining.</returns>
    public CompoundDefinition Filter(ISearchDefinition definition) => Filter(definition.Render());

    /// <summary>
    /// In a query with multiple should clauses, you can use the miniumumShouldMatch option to specify a minimum number of clauses which must match to return a result.
    /// </summary>
    /// <value>The minimum number of <see cref="Should(ISearchDefinition)"/> clauses that must match the document to be returned.</value>
    public int MinimumShouldMatch { get; set; }

    BsonDocument ISearchDefinition.Render()
    {
        var document = new BsonDocument();
        if (_mustClauses.Any())
        {
            document.Add("must", new BsonArray(_mustClauses));
        }
        if (_mustNotClauses.Any())
        {
            document.Add("mustNot", new BsonArray(_mustNotClauses));
        }

        if (_shouldClauses.Any())
        {
            document.Add("should", new BsonArray(_shouldClauses));
        }
        if (_filterClauses.Any())
        {
            document.Add("filter", new BsonArray(_filterClauses));
        }

        if (MinimumShouldMatch > 0)
        {
            document.Add("minimumShouldMatch", MinimumShouldMatch);
        }

        return new BsonDocument("compound", document);
    }
}
