using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Realms.Search;

namespace Realm.Search.Demo.Models;

#nullable enable

public partial class Movie2 // : ISearchModel<Movie2.Projection>
{
    public static Projection DefaultProjection => Projection.Default;

    [BsonElement("searchHighlights")]
    public Highlight[]? SearchHighlights { get; set; }

    public class Projection : ProjectionModel
    {
        public static readonly Projection Default = new()
        {
            Title = true,
        };

        public static readonly Projection NoId = new()
        {
            Title = true,
            ExtraExpressions = new()
            {
                ["_id"] = false
            }
        };

        [BsonElement("title")]
        public bool Title { get; set; }
    }
}
