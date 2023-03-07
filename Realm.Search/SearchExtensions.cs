using Realms.Search;

namespace Realms.Sync;

/// <summary>
/// An extension method on <see cref="MongoClient.Collection{TDocument}"/> that constructs a <see cref="SearchClient{TModel}"/>
/// </summary>
public static class SearchExtensions
{
    /// <summary>
    /// Get a <see cref="SearchClient{TModel}"/> for this <paramref name="collection"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of documents contained in the collection.</typeparam>
    /// <param name="collection">The remote mongo collection.</param>
    /// <param name="index">The search index to use - the default one is <c>default</c>.</param>
    /// <returns>A <see cref="SearchClient{TModel}"/> instance scoped for the provided collection.</returns>
    public static SearchClient<TModel> Search<TModel>(this MongoClient.Collection<TModel> collection, string? index = null)
        where TModel : class, ISearchModel
        => new(collection, index);
}
