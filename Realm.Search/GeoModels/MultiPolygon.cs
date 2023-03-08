using System.Linq;
using MongoDB.Bson;

namespace Realms.Search.Geo;

/// <summary>
/// Represents a multipolygon that can be used to construct a <see cref="GeoWithinDefinition"/>.
/// </summary>
public class MultiPolygon : GeoJson
{
    /// <summary>
    /// The polygons that comprise the multipolygon.
    /// </summary>
    /// <value>The polygons that comprise the multipolygon.</value>
    public Polygon[] Polygons { get; }

    /// <inheritdoc/>
    protected override string Type => throw new System.NotImplementedException();

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiPolygon"/> class.
    /// </summary>
    /// <param name="polygons">The polygons that comprise the multipolygon.</param>
    public MultiPolygon(params Polygon[] polygons)
    {
        Argument.Ensure(polygons.Length > 0, "At least one polygon must be provided.");

        Polygons = polygons;
    }

    /// <inheritdoc/>
    public override BsonArray RenderCoordinates() => new BsonArray(Polygons.Select(p => p.RenderCoordinates()));
}
