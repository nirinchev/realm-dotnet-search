using System.Linq;
using MongoDB.Bson;

namespace Realms.Search.Geo;

/// <summary>
/// Represents a polygon that can be used to construct a <see cref="GeoWithinDefinition"/>.
/// </summary>
/// <seealso href="https://www.mongodb.com/docs/upcoming/reference/geojson/#polygon"/>
public class Polygon : GeoJson
{
    /// <summary>
    /// The ring(s) forming the polygon.
    /// </summary>
    public LineString[] Rings { get; }

    /// <inheritdoc/>
    protected override string Type => "Polygon";

    /// <summary>
    /// Initializes a new instance of the <see cref="Polygon"/> class.
    /// </summary>
    /// <param name="rings">
    /// The ring(s) forming the polygon. These are closed line strings of 3 or more points where the first
    /// and the last point are the same.
    /// </param>
    /// <remarks>
    /// For Polygons with a single ring, the ring cannot self-intersect.
    /// <br/>
    /// For Polygons with multiple rings:
    /// <list type="bullet">
    /// <item><description>The first described ring must be the exterior ring.</description></item>
    /// <item><description>The exterior ring cannot self-intersect.</description></item>
    /// <item><description>Any interior ring must be entirely contained by the outer ring.</description></item>
    /// <item><description>Interior rings cannot intersect or overlap each other. Interior rings cannot share an edge.</description></item>
    /// </list>
    /// </remarks>
    public Polygon(params LineString[] rings)
    {
        Argument.Ensure(rings.Length > 0, "At least one closed linear ring must be provided.");

        foreach (var ring in rings)
        {
            Argument.Ensure(ring.Points.Length > 3, "There must be at least 4 points for the shape to be considered a ring.");
            Argument.Ensure(ring.Points.First().Equals(ring.Points.Last()), "The first and the last points must be equal for the ring to be closed.");
        }

        Rings = rings;
    }

    /// <inheritdoc/>
    public override BsonArray RenderCoordinates() => new(Rings.Select(r => r.RenderCoordinates()));
}
