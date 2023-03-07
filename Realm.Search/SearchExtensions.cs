using Realms.Search;

namespace Realms.Sync;

public static class SearchExtensions
{
    public static SearchClient<TModel> Search<TModel>(this MongoClient.Collection<TModel> collection, string? index = null)
        where TModel : class, ISearchModel
        => new(collection, index);
}
