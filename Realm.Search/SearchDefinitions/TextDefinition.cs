using System;
using MongoDB.Bson;

namespace Realms.Search
{
    /// <summary>
    /// The text operator performs a full-text search using the analyzer that you specify in the index configuration.
    /// If you omit an analyzer, the text operator uses the default standard analyzer.
    /// </summary>
    /// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/text/"/>
	public class TextDefinition : FuzzySearchDefinitionBase
    {
        /// <inheritdoc/>
        protected override string OperatorName => "text";

        /// <summary>
        /// Name of the synonym mapping definition in the index definition. Value can't be an empty string. You can't use fuzzy with synonyms.
        /// </summary>
        /// <value>The synonym mapping to be used when searching.</value>
        /// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/synonyms/#std-label-synonyms-ref"/>
        public string? Synonyms { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextDefinition"/> class.
        /// </summary>
        /// <param name="query">The string to search for.</param>
        /// <param name="path">The field path in the document.</param>
        /// <param name="fuzzy">The fuzzy search options.</param>
        /// <param name="score">The score assigned to search matches.</param>
        public TextDefinition(QueryDefinition query, PathDefinition path, FuzzyOptions? fuzzy = null, ScoreOptions? score = null)
            : base(query, path, score, fuzzy)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="TextDefinition"/> class.
        /// </summary>
        /// <param name="query">The string to search for.</param>
        /// <param name="path">The field path in the document.</param>
        /// <param name="synonyms">The synonym mapping to use when searching.</param>
        /// <param name="score">The score assigned to search matches.</param>
        public TextDefinition(QueryDefinition query, PathDefinition path, string synonyms, ScoreOptions? score = null)
            : base(query, path, score, fuzzy: null)
        {
            Synonyms = synonyms;
        }

        /// <inheritdoc/>
        protected override void PopulateDefinition(BsonDocument baseDefinition)
        {
            base.PopulateDefinition(baseDefinition);

            if (Synonyms != null)
            {
                baseDefinition["synonyms"] = Synonyms;
            }
        }
    }
}

