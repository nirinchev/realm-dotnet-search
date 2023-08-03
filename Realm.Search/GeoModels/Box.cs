using MongoDB.Bson;

namespace Realms.Search.Geo;

/// <summary>
/// Represents a box geometry that can be used to construct a <see cref="GeoWithinDefinition"/>.
/// </summary>
public class Box
{
    /// <summary>
    /// The bottom left corner of the box.
    /// </summary>
    /// <value>The bottom left corner.</value>
    public Point BottomLeft { get; }

    /// <summary>
    /// The top right corner of the box.
    /// </summary>
    /// <value>The top right corner.</value>
    public Point TopRight { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Box"/> class.
    /// </summary>
    /// <param name="bottomLeft">The bottom left corner.</param>
    /// <param name="topRight">The top right corner.</param>
    public Box(Point bottomLeft, Point topRight)
    {
        BottomLeft = bottomLeft;
        TopRight = topRight;
    }

    internal BsonDocument Render() => new()
    {
        ["bottomLeft"] = BottomLeft.Render(),
        ["topRigth"] = TopRight.Render(),
    };
}
