using MongoDB.Bson;

namespace Realms.Search;

/// <summary>
/// The Atlas Search highlight option adds fields to the result set that display search terms in their original context.
/// You can use it in conjunction with all <c>$search</c> operators to display search terms as they appear in the returned
/// documents, along with the adjacent text content (if any).
/// </summary>
/// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/highlighting/"/>
public class HighlightOptions
{
    /// <summary>
    /// The document field to search.
    /// </summary>
    /// <value>The document field to search.</value>
    public string Path { get; }

    /// <summary>
    /// Maximum number of characters to examine on a document when performing highlighting for a field.
    /// If omitted, defaults to 500,000, which means that Atlas Search only examines the first 500,000
    /// characters in the search field in each document for highlighting.
    /// </summary>
    /// <value>The maximum number of characters to examine.</value>
    public int MaxCharactersToExamine { get; }

    /// <summary>
    /// Number of high-scoring passages to return per document in the highlights results for each field.
    /// A passage is roughly the length of a sentence. If omitted, defaults to 5, which means that for each document,
    /// Atlas Search returns the top 5 highest-scoring passages that match the search text.
    /// </summary>
    /// <value>The maximum number of passages to return.</value>
    public int MaxNumPassages { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HighlightOptions"/> class.
    /// </summary>
    /// <param name="path">The document field to search.</param>
    /// <param name="maxCharactersToExamine">The maximum number of characters to examine.</param>
    /// <param name="maxNumPassages">The maximum number of passages to return.</param>
    public HighlightOptions(string path, int maxCharactersToExamine = 500_000, int maxNumPassages = 5)
    {
        Path = path;
        MaxCharactersToExamine = maxCharactersToExamine;
        MaxNumPassages = maxNumPassages;
    }

    internal BsonDocument Render()
    {
        var result = new BsonDocument("path", Path);

        if (MaxCharactersToExamine != 500_000)
        {
            result["maxCharactersToExamine"] = MaxCharactersToExamine;
        }

        if (MaxNumPassages != 5)
        {
            result["maxNumPassages"] = MaxNumPassages;
        }

        return result;
    }
}
