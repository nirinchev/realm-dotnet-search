using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace Realm.Search;

public class HighlightOptions
{
    public string Path { get; }

    public int? MaxCharactersToExamine { get; set; }

    public int? MaxNumPassages { get; set; }

    public HighlightOptions(string path)
    {
        Path = path;
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

/// <summary>
/// Represents a result of highlighting.
/// </summary>
public class Highlight
{
    /// <summary>
    /// Gets or sets the document field which returned a match.
    /// </summary>
    [BsonElement("path")]
    public string Path { get; set; } = null!;

    /// <summary>
    /// Gets or sets one or more objects containing the matching text and the surrounding text
    /// (if any).
    /// </summary>
    [BsonElement("texts")]
    public HighlightText[] Texts { get; set; } = null!;

    /// <summary>
    /// Gets or sets the score assigned to this result.
    /// </summary>
    [BsonElement("score")]
    public double Score { get; set; }
}

/// <summary>
/// Represents the matching text or the surrounding text of a highlighting result.
/// </summary>
public class HighlightText
{
    /// <summary>
    /// Gets or sets the text from the field which returned a match.
    /// </summary>
    [BsonElement("value")]
    public string Value { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type of text, matching or surrounding.
    /// </summary>
    [BsonElement("type")]
    [BsonSerializer(typeof(HighlightTextTypeSerializer))]
    public HighlightTextType Type { get; set; }
}

/// <summary>
/// Represents the type of text in a highlighting result, matching or surrounding.
/// </summary>
public enum HighlightTextType
{
    /// <summary>
    /// Indicates that the text contains a match.
    /// </summary>
    Hit,

    /// <summary>
    /// Indicates that the text contains the text content adjacent to a matching string.
    /// </summary>
    Text
}

/// <summary>
/// Represents a serializer for a <see cref="HighlightTextType"/> value.
/// </summary>
internal class HighlightTextTypeSerializer : SerializerBase<HighlightTextType>
{
    private const string hitName = "hit";
    private const string textName = "text";

    /// <summary>
    /// Deserializes a value.
    /// </summary>
    /// <param name="context">The deserialization context.</param>
    /// <param name="args">The deserialization arguments.</param>
    /// <returns>The value.</returns>
    public override HighlightTextType Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var type = context.Reader.GetCurrentBsonType();
        if (type != BsonType.String)
        {
            throw CreateCannotDeserializeFromBsonTypeException(type);
        }
        string value = context.Reader.ReadString();
        switch (value)
        {
            case hitName:
                return HighlightTextType.Hit;
            case textName:
                return HighlightTextType.Text;
            default:
                throw new NotSupportedException($"Unexpected representation string for HiglightType: {value}");
        }
    }

    /// <summary>
    /// Serializes a value.
    /// </summary>
    /// <param name="context">The serialization context.</param>
    /// <param name="args">The serialization arguments.</param>
    /// <param name="value">The value.</param>
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, HighlightTextType value)
    {
        switch (value)
        {
            case HighlightTextType.Hit:
                context.Writer.WriteString(hitName);
                break;
            case HighlightTextType.Text:
                context.Writer.WriteString(textName);
                break;
            default:
                throw new NotSupportedException($"Unexpected HighlightTextType value: {value}");
        }
    }
}
