using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Realm.Search;

namespace Realms.Search
{
	public interface ISearchModel
	{
        Highlight[]? SearchHighlights { get; set; }
    }

    public interface ISearchModel<out T> : ISearchModel
		where T : ProjectionModel
	{
    }

    public abstract class ProjectionModel
	{
        [BsonIgnore]
        public bool SearchHighlights { get; set; }

        [BsonIgnore]
        public bool TextScore { get; set; }

        [BsonIgnore]
        public bool SearchScore { get; set; }

        [BsonIgnore]
        public BsonDocument? ExtraExpressions { get; set; }

        public virtual BsonDocument Render()
		{
            var result = this.ToBsonDocument();
            result.Remove("_t");

            if (ExtraExpressions != null)
            {
                foreach (var kvp in ExtraExpressions)
                {
                    result[kvp.Name] = kvp.Value;
                }
            }

            if (SearchHighlights)
            {
                result["searchHighlights"] = new BsonDocument("$meta", "searchHighlights");
            }

            if (TextScore)
            {
                result["textScore"] = new BsonDocument("$meta", "textScore");
            }

            if (SearchScore)
            {
                result["searchScore"] = new BsonDocument("$meta", "searchScore");
            }

            return result;
        }
	}
}
