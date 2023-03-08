using System.Linq;
using MongoDB.Bson;

namespace Realms.Search.Geo;

/// <summary>
/// Represents a linear (closed) ring of points.
/// </summary>
/// <seealso href="https://www.mongodb.com/docs/upcoming/reference/geojson/#polygon"/>
public class LineString : GeoJson
{
    /// <summary>
    /// The points representing the line string.
    /// </summary>
    public Point[] Points { get; }

    /// <inheritdoc/>
    protected override string Type => "LineString";

    /// <summary>
    /// Initializes a new instance of the <see cref="LineString"/> class.
    /// </summary>
    /// <param name="points">The collection of points that form the line string.</param>
    public LineString(params Point[] points)
    {
        Points = points;
    }

    /// <inheritdoc/>
    public override BsonArray RenderCoordinates() => new(Points.Select(p => p.RenderCoordinates()));
}
