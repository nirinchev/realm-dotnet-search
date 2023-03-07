using System;
using System.IO;
using MongoDB.Bson;
using Realms.Search;

namespace Realm.Search
{
    internal interface ISearchDefinition
    {
        BsonDocument Render();
    }
}

