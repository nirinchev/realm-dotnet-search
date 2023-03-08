using System.Diagnostics;
using System.Text.Json;
using MongoDB.Bson;
using Realm.Search.Demo.Models;
using Realms.Sync;
using RealmApp = Realms.Sync.App;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using MongoDB.Bson.Serialization.Attributes;
using Realms.Search;
using Realms.Search.Geo;

namespace Realm.Search.Demo.Services;

public static class SearchService
{
	private static RealmApp _app = null!;
	private static MongoClient.Collection<Movie> _movieCollection = null!;
	private static MongoClient.Collection<Listing> _listingCollection = null!;

	public static async Task Initialize()
	{
        using var configStream = await FileSystem.Current.OpenAppPackageFileAsync("config.json");

		var config = JsonSerializer.Deserialize<Config>(configStream)!;

		_app = RealmApp.Create(new AppConfiguration(config.appId ?? throw new Exception("Make sure to fill in your app id in config.json"))
		{
			BaseUri = new Uri(config.serverUrl)
		});

		var user = _app.CurrentUser ?? await _app.LogInAsync(Credentials.Anonymous());

		var client = user.GetMongoClient(config.serviceName);

        _movieCollection = client.GetDatabase("sample_mflix")
			.GetCollection<Movie>("movies");

		_listingCollection = client.GetDatabase("sample_airbnb")
			.GetCollection<Listing>("listingsAndReviews");
    }

    public static async Task<Movie[]> Autocomplete(string query)
	{
        return await _movieCollection.Search().Autocomplete(new(query, "title"), highlightOptions: new("title"), limit: 10);
    }

    public static async Task<Listing[]> Compound(string query, Location center, double distance)
	{
		var projection = Listing.Projection.NoId;
		projection.Address = false;
		projection.ExtraExpressions!.Add("address.location", true);
		projection.ExtraExpressions!.Add("address.street", true);

		var geoCircle = new Circle(new(center.Latitude, center.Longitude), radius: distance);
		var definition = new CompoundDefinition()
			.Must(new GeoWithinDefinition(geoCircle, path: "address.location"))
			.Should(new PhraseDefinition(query, "description"));
	
		return await _listingCollection.Search().Compound(definition, projection: projection, highlightOptions: new("description"), limit: 10);
	}

	private class Config
	{
		public string? appId { get; set; }

		public string serverUrl { get; set; } = "https://realm-qa.mongodb.com";

		public string serviceName { get; set; } = "https://realm-qa.mongodb.com";
    }
}
