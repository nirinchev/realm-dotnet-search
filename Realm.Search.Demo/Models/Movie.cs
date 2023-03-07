using MongoDB.Bson.Serialization.Attributes;
using Realms.Search;

namespace Realm.Search.Demo.Models;

[BsonIgnoreExtraElements]
public partial class Movie : ISearchModel
{
    [BsonElement("title")]
    public string Title { get; set; } = null!;
}
