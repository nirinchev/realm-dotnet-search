using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Realm.Search;

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
    [BsonSerializer(typeof(LowercaseEnumSerializer<HighlightTextType>))]
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
