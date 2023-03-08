using MongoDB.Bson;

namespace Realms.Search.Geo;

/// <summary>
/// Represents a circle geometry that can be used to construct a <see cref="GeoWithinDefinition"/>.
/// </summary>
public class Circle
{
    /// <summary>
    /// The center of the circle.
    /// </summary>
    /// <value>The circle center.</value>
    public Point Center { get; }

    /// <summary>
    /// The radius of the circle.
    /// </summary>
    /// <value>The radius of the circle in meters.</value>
    public double Radius { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Circle"/> class.
    /// </summary>
    /// <param name="center">The circle center.</param>
    /// <param name="radius">The radius of the circle in meters.</param>
    public Circle(Point center, double radius)
    {
        Center = center;
        Radius = radius;
    }

    internal BsonDocument Render()
    {
        return new()
        {
            ["center"] = Center.Render(),
            ["radius"] = Radius
        };
    }
}
