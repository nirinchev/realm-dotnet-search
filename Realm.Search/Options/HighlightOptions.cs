using MongoDB.Bson;

namespace Realm.Search;

public class HighlightOptions
{
    public string Path { get; }

    public int? MaxCharactersToExamine { get; }

    public int? MaxNumPassages { get; }

    public HighlightOptions(string path, int? maxCharactersToExamine = null, int? maxNumPassages = null)
    {
        Path = path;
        MaxCharactersToExamine = maxCharactersToExamine;
        MaxNumPassages = maxNumPassages;
    }

    internal BsonDocument Render()
    {
        var result = new BsonDocument("path", Path);

        if (MaxCharactersToExamine != null)
        {
            result["maxCharactersToExamine"] = MaxCharactersToExamine.Value;
        }

        if (MaxNumPassages != null)
        {
            result["maxNumPassages"] = MaxNumPassages.Value;
        }

        return result;
    }
}
