# Realm .NET Search Extensions

Realm.Search is a set of extension API that simplify running Atlas Search aggregations against Atlas App Services.

## Installation

The Realm.Search package is available on NuGet. To install it, run the following command in the Package Manager Console:

```powershell
dotnet add package Realm
```

## Prerequisites

1. You need an Atlas Cluster with a configured Search index. You can learn more about how to configure Atlas Search in the [Atlas Search documentation](https://docs.atlas.mongodb.com/reference/atlas-search/).
1. You need an Atlas App Services application connected to the Atlas Cluster with a configured [schema](https://www.mongodb.com/docs/atlas/app-services/schemas/) and [rules](https://www.mongodb.com/docs/atlas/app-services/rules/).

## Docs

You can find the API docs [here](https://nirinchev.github.io/realm-dotnet-search/).

## Usage

The main entrypoint for the Search API is the [Search](xref:Realms.Sync.SearchExtensions) extension method defined on [MongoClient.Collection](https://www.mongodb.com/docs/realm-sdks/dotnet/latest/reference/Realms.Sync.MongoClient.Collection-1.html). It returns a [SearchClient](xref:Realms.Search.SearchClient`1) that can be used to run aggregations against the remote MongoDB data.

```csharp
// The built-in source generator will process all classes that implement ISearchModel
// and generate an implementation and a projection model to select which properties will
// be returned
class Movie : ISearchModel
{
    [BsonElement("_id")))]
    public ObjectId Id { get; set; }

    [BsonElement("title"))]
    public string Title { get; set; }

    [BsonElement("description"))]
    public string Description { get; set; }
}

var app = App.Create("your-app-id");
var user = await app.LogInAsync(Credentials.Anonymous());

var searchClient = user.GetMongoClient("mongodb-atlas")
    .GetDatabase("my-database")
    .GetCollection<Movie>("movies")
    .Search();

var autoCompleteResults = await searchClient.AutoComplete(new("star wars", "title"));

// By default all fields are returned - you can modify this by creating a projection
var projection = Movie.Projection.NoId; // Returns all fields on the Movie model exept for the Id
projection.Description = false; // Removes the description field from the projection

var autoCompleteResults = await searchClient.AutoComplete(new("star wars", "title"), projection);

// You can also provide options for highlighting matches
var highlight = new HighlightOptions("title");
var autoCompleteResults = await searchClient.AutoComplete(new("star wars", "title"), highlightOptions: highlight);
```

## Demo

The Realm.Search.Demo project contains a simple MAUI application that demoes some of the features of the library. It's querying against the sample datasets provided by MongoDB.

### Running the demo

1. Setup an Atlas Cluster.
1. Create an Atlas App Services application and connect it to the Atlas Cluster.
  1. The demo uses the `sample_airbnb/listingsAndReviews` and `sample_mflix/movies` collections.
  1. Define the schema for the collections - you can use the Generate Schema functionality in the Atlas App Services UI.
  1. Define rules for the collections - we're only going to be reading data, so `readAll` permissions are sufficient.
1. Update `config.json` with your app id.
1. Follow the [Define an Index with Autocomplete](https://www.mongodb.com/docs/atlas/atlas-search/tutorial/autocomplete-tutorial/#define-an-index-with-autocomplete) tutorial to create an Atlas Search index.
1. Follow the [Create the Atlas Search Index](https://www.mongodb.com/docs/atlas/atlas-search/tutorial/run-geo-query/#create-the-fts-index) tutorial to create a Geo Search index.
1. (On Android only) The Compound sample uses a map control, which on Android requires a Google Maps API key. Follow the instructions on the [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/map?view=net-maui-7.0#get-a-google-maps-api-key) to get one and update AndroidManifest.xml with it.

## FAQ

* Is this library stable?
  * The functionality is stable, but the API is still very much in flux. We will be adding more features and improving the API as we go. In it's current state it is likely to cover some straightforward use cases, but you should expect to run into limitations and missing functionality.
* Will you be actively maintaining this library?
  * This is a proof of concept project and we will not be actively developing it. We will be happy to add features and fix bugs as they are reported, but we won't be adding new functionality on a regular cadence.
* I need a feature, can I contribute?
  * Absolutely! The library is open source and as long as the feature is well thought out and fits with the overall design of the library we will be happy to accept contributions.