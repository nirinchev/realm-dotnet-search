using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using MongoDB.Bson;
using Realms.Search.Geo;

namespace Realms.Search;

/// <summary>
/// The geoWithin operator supports querying geographic points within a given geometry. Only points are returned, even if <c>indexShapes</c> value is true in the index definition.
/// </summary>
/// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/geoWithin/"/>
public class GeoWithinDefinition : SearchDefinitionBase
{
    private readonly string _type;
    private readonly BsonDocument _geometry;

    /// <inheritdoc/>
    protected override string OperatorName => "geoWithin";

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoWithinDefinition"/> class..
    /// </summary>
    /// <param name="circle">The circle within which to search for results.</param>
    /// <param name="path">The field path in the document.</param>
    /// <param name="score">The score assigned to search matches.</param>
    public GeoWithinDefinition(Circle circle, PathDefinition path, ScoreOptions? score = null)
        : this("circle", circle.Render(), path, score) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoWithinDefinition"/> class..
    /// </summary>
    /// <param name="box">The box within which to search for results.</param>
    /// <param name="path">The field path in the document.</param>
    /// <param name="score">The score assigned to search matches.</param>
    public GeoWithinDefinition(Box box, PathDefinition path, ScoreOptions? score = null)
        : this("box", box.Render(), path, score) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoWithinDefinition"/> class..
    /// </summary>
    /// <param name="polygon">The polygon within which to search for results.</param>
    /// <param name="path">The field path in the document.</param>
    /// <param name="score">The score assigned to search matches.</param>
    public GeoWithinDefinition(Polygon polygon, PathDefinition path, ScoreOptions? score = null)
        : this("geometry", polygon.Render(), path, score) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoWithinDefinition"/> class..
    /// </summary>
    /// <param name="multiPolygon">The multiPolygon within which to search for results.</param>
    /// <param name="path">The field path in the document.</param>
    /// <param name="score">The score assigned to search matches.</param>
    public GeoWithinDefinition(MultiPolygon multiPolygon, PathDefinition path, ScoreOptions? score = null)
        : this("geometry", multiPolygon.Render(), path, score) { }

    private GeoWithinDefinition(string type, BsonDocument geometry, PathDefinition path, ScoreOptions? score)
        : base(path, score)
    {
        _type = type;
        _geometry = geometry;
    }

    /// <inheritdoc/>
    protected override void PopulateDefinition(BsonDocument baseDefinition)
    {
        baseDefinition[_type] = _geometry;
    }
}
