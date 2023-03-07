using System.Diagnostics;
using System.Text.Json;
using Realm.Search.Demo.Models;
using Realms.Sync;
using RealmApp = Realms.Sync.App;

namespace Realm.Search.Demo.Services;

public static class SearchService
{
	private static RealmApp _app = null!;
	private static MongoClient.Collection<Movie> _collection = null!;

	public static async Task Initialize()
	{
        using var configStream = await FileSystem.Current.OpenAppPackageFileAsync("config.json");

		var config = JsonSerializer.Deserialize<Config>(configStream)!;

		_app = RealmApp.Create(new AppConfiguration(config.appId ?? throw new Exception("Make sure to fill in your app id in config.json"))
		{
			BaseUri = new Uri(config.serverUrl)
		});

		var user = _app.CurrentUser ?? await _app.LogInAsync(Credentials.Anonymous());
		_collection = user.GetMongoClient(config.serviceName)
			.GetDatabase(config.databaseName)
			.GetCollection<Movie>("movies");
    }

	public static async Task<Movie[]> Autocomplete(string query)
	{
		return await _collection.Autocomplete(query, "title", highlightOptions: new("title"), limit: 10);
	}

	private class Config
	{
		public string? appId { get; set; }

		public string serverUrl { get; set; } = "https://realm-qa.mongodb.com";

		public string serviceName { get; set; } = "https://realm-qa.mongodb.com";

		public string databaseName { get; set; } = "sample_mflix";
    }
}
