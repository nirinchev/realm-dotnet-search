using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace Realms.Search
{
	public class QueryDefinition
	{
		internal BsonValue Value { get; }

		private QueryDefinition(BsonValue value)
		{
			Value = value;
		}

		public static implicit operator QueryDefinition(string query)
			=> new QueryDefinition(new BsonString(query));

		public static implicit operator QueryDefinition(string[] queryTerms)
			=> new QueryDefinition(new BsonArray(queryTerms.Select(t => new BsonString(t))));
	}
}

