using System;
using System.IO;
using MongoDB.Bson;
using Realms.Search;

namespace Realms.Search;

internal interface ISearchDefinition
{
    BsonDocument Render();
}

