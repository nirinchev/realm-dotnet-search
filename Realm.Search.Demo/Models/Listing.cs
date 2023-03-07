using System;
using MongoDB.Bson.Serialization.Attributes;
using Realms.Search;

using MauiLocation = Microsoft.Maui.Devices.Sensors.Location;

namespace Realm.Search.Demo.Models;

[BsonIgnoreExtraElements]
public partial class Listing : ISearchModel
{
    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("description")]
    public string Description { get; set; } = null!;

    [BsonElement("address")]
    public Address Address { get; set; } = null!;
}

[BsonIgnoreExtraElements]
public class Address
{
    [BsonElement("street")]
    public string Street { get; set; } = null!;

    [BsonElement("location")]
    public Location Location { get; set; } = null!;

    public string StringAddress => $"📌 {Street}";

    public MauiLocation MauiLocation => new(Location.Latitude, Location.Longitude);
}

[BsonIgnoreExtraElements]
public class Location
{
    [BsonElement("coordinates")]
    private double[] Coordinates { get; set; } = null!;

    public double Longitude => Coordinates[0];

    public double Latitude => Coordinates[1];
}