using System;
using MongoDB.Bson;

namespace Realms.Search.Geo;

/// <summary>
/// The base class for all types representing a GeoJSON object.
/// </summary>
public abstract class GeoJson
{
    /// <summary>
    /// The type of the object.
    /// </summary>
	protected abstract string Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoJson"/> class.
    /// </summary>
	protected GeoJson()
	{
	}

    /// <summary>
    /// Render the object's coordinates.
    /// </summary>
    /// <returns>A BsonArray representing the object's coordinates.</returns>
	public abstract BsonArray RenderCoordinates();

    /// <summary>
    /// Render the document as GeoJSON.
    /// </summary>
    /// <returns>A BsonDocument representing the object.</returns>
    public BsonDocument Render() => new()
    {
        ["type"] = Type,
        ["coordinates"] = RenderCoordinates()
    };
}

