using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Realm.Search
{
    /// <summary>
    /// Enable fuzzy search. Find strings which are similar to the search term or terms.
    /// </summary>
    public class FuzzyOptions
	{
        /// <summary>
        /// Gets or sets the maximum number of single-character edits required to match the specified search term. Value can be 1 or 2.
		/// The default value is 2. Uses <see href="https://en.wikipedia.org/wiki/Damerau–Levenshtein_distance">Damerau-Levenshtein distance</see>.
        /// </summary>
        /// <value>The maximum number of edits.</value>
        public int MaxEdits { get; set; } = 2;

        /// <summary>
        /// Gets or sets the number of characters at the beginning of each term in the result that must exactly match. The default value is 0.
        /// </summary>
        /// <value>The prefix length.</value>
        public int PrefixLength { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum number of variations to generate and search for. This limit applies on a per-token basis. The default value is 50.
        /// </summary>
        /// <value>The maximum number of expansions.</value>
		public int MaxExpansions { get; set; } = 50;

		internal BsonDocument Render()
		{
			var result = new BsonDocument();
			if (MaxEdits != 2)
			{
				result["maxEdits"] = MaxEdits;
			}

			if (PrefixLength != 0)
			{
                result["prefixLength"] = PrefixLength;
            }

			if (MaxExpansions != 50)
			{
                result["maxExpansions"] = MaxExpansions;
            }

			return result;
        }
    }
}

