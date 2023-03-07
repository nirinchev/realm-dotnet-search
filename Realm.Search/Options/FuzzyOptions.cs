using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Realm.Search
{
	public class FuzzyOptions
	{
		public int MaxEdits { get; set; } = 2;

		public int PrefixLength { get; set; } = 0;

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

