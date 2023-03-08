using MongoDB.Bson;

namespace Realms.Search.Geo;

/// <summary>
/// A class representing a point.
/// </summary>
/// <seealso href="https://www.mongodb.com/docs/upcoming/reference/geojson/#point"/>
public class Point : GeoJson
{
    /// <inheritdoc/>
    protected override string Type => "Point";

    private double _latitude;
    private double _longitude;

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> class.
    /// </summary>
    /// <param name="latitude">The latitude of the point.</param>
    /// <param name="longitude">The longitude of the point.</param>
    public Point(double latitude, double longitude)
    {
        _latitude = latitude;
        _longitude = longitude;
    }

    /// <inheritdoc/>
    public override BsonArray RenderCoordinates() => new() { _longitude, _latitude };

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        if (obj is Point point)
        {
            return _latitude == point._latitude
                && _longitude == point._longitude;
        }

        return false;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return _latitude.GetHashCode() * 17 + _longitude.GetHashCode();
    }
}
