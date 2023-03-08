using System;
using MongoDB.Bson;

namespace Realms.Search;

/// <summary>
/// The phrase operator performs search for documents containing an ordered sequence of terms.
/// </summary>
public class PhraseDefinition : QuerySearchDefinitionBase
{
    /// <inheritdoc/>
    protected override string OperatorName => "phrase";

    /// <summary>
    /// Allowable distance between words in the <see cref="QuerySearchDefinitionBase.Query"/> phrase.
    /// </summary>
    /// <remarks>
    /// Lower value allows less positional distance between the words and greater value allows more
    /// reorganization of the words and more distance between the words to satisfy the query. The
    /// default is 0, meaning that words must be exactly in the same position as the query in order
    /// to be considered a match. Exact matches are scored higher.
    /// </remarks>
    /// <value>The max allowed distance between words.</value>
    public int Slop { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PhraseDefinition"/> class..
    /// </summary>
    /// <param name="query">The string to search for.</param>
    /// <param name="path">The field path in the document.</param>
    /// <param name="score">The score assigned to search matches.</param>
    /// <param name="slop">The maximum allowed distance between words.</param>
    public PhraseDefinition(QueryDefinition query, PathDefinition path, ScoreOptions? score = null, int slop = 0)
        : base(query, path, score)
    {
        Slop = slop;
    }

    /// <inheritdoc/>
    protected override void PopulateDefinition(BsonDocument baseDefinition)
    {
        base.PopulateDefinition(baseDefinition);

        if (Slop != 0)
        {
            baseDefinition["slop"] = Slop;
        }
    }
}
