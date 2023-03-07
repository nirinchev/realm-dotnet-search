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
    /// <summary>
    /// Clauses that must match to for a document to be included in the results. The returned score is the sum
    /// of the scores of all the subqueries in the clause.
    /// </summary>
    /// <remarks>Maps to the AND boolean operator.</remarks>
    /// <value>The clauses that must match the document.</value>
		public List<BsonDocument> MustClauses { get; set; } = new();

    /// <summary>
    /// Clauses that you prefer to match in documents that are included in the results. Documents that contain a
    /// match for a should clause have higher scores than documents that don't contain a should clause. The returned
    /// score is the sum of the scores of all the subqueries in the clause. If you use more than one should clause,
    /// you can use the minimumShouldMatch option to specify a minimum number of should clauses that must match to
    /// include a document in the results.If omitted, the minimumShouldMatch option defaults to 0.
    /// </summary>
    /// <remarks>Maps to the OR boolean operator.</remarks>
    /// <value>The clauses that should match the document.</value>
		public List<BsonDocument> ShouldClauses { get; set; } = new();

    /// <summary>
    /// Clauses that must not match for a document to be included in the results. mustNot clauses don't contribute to a returned document's score.
    /// </summary>
    /// <remarks>Maps to the AND NOT boolean operator.</remarks>
    /// <value>The clauses that must not match the document.</value>
    public List<BsonDocument> MustNotClauses { get; set; } = new();

    /// <summary>
    /// Clauses that must all match for a document to be included in the results. filter clauses do not contribute to a returned document's score.
    /// </summary>
    /// <value>The filter clauses.</value>
    public List<BsonDocument> FilterClauses { get; set; } = new();

    /// <summary>
    /// In a query with multiple should clauses, you can use the miniumumShouldMatch option to specify a minimum number of clauses which must match to return a result.
    /// </summary>
    /// <value>The minimum number of <see cref="ShouldClauses"/> that must match the document to be returned.</value>
    public int MinimumShouldMatch { get; set; }

    BsonDocument ISearchDefinition.Render()
    {
        var document = new BsonDocument();
        if (MustClauses?.Any() == true)
        {
            document.Add("must", new BsonArray(MustClauses));
        }
        if (MustNotClauses?.Any() == true)
        {
            document.Add("mustNot", new BsonArray(MustNotClauses));
        }

        if (ShouldClauses?.Any() == true)
        {
            document.Add("should", new BsonArray(ShouldClauses));
        }
        if (FilterClauses?.Any() == true)
        {
            document.Add("filter", new BsonArray(FilterClauses));
        }

        if (MinimumShouldMatch > 0)
        {
            document.Add("minimumShouldMatch", MinimumShouldMatch);
        }

        return new BsonDocument("compound", document);
    }
}
