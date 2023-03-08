using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace Realms.Search
{
	/// <summary>
	/// A <see cref="QueryDefinition"/> represents one or several search expressions.
	/// </summary>
	public class QueryDefinition
	{
		internal BsonValue Value { get; }

		private QueryDefinition(BsonValue value)
		{
			Value = value;
		}

		/// <summary>
		/// An operator that converts a search query to a <see cref="QueryDefinition"/>.
		/// </summary>
		/// <param name="query">The query that Atlas will search for.</param>
		public static implicit operator QueryDefinition(string query)
			=> new(new BsonString(query));

        /// <summary>
        /// An operator that converts a list of expressions to a <see cref="QueryDefinition"/>.
        /// </summary>
        /// <param name="queryTerms">
		/// The query terms that Atlas will search for. If there are multiple terms in a string,
		/// Atlas Search also looks for a match for each term in the string separately.
		/// </param>
        public static implicit operator QueryDefinition(string[] queryTerms)
			=> new(new BsonArray(queryTerms.Select(t => new BsonString(t))));
	}
}
