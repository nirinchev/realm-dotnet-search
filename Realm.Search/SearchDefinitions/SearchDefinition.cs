using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using Realms.Search;

namespace Realm.Search
{
    internal interface ISearchDefinition
    {
        BsonDocument Render();
    }

    public class CompoundDefinition : ISearchDefinition
	{
		public List<BsonDocument> MustClauses { get; set; } = new();

		public List<BsonDocument> ShouldClauses { get; set; } = new();

		public List<BsonDocument> MustNotClauses { get; set; } = new();

		public List<BsonDocument> FilterClauses { get; set; } = new();

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
}

