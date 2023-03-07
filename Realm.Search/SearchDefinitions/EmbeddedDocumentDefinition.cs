using System;
using MongoDB.Bson;

namespace Realm.Search
{
    /// <summary>
    /// TODO: document me
    /// </summary>
	public class EmbeddedDocumentDefinition : ISearchDefinition
	{
        BsonDocument ISearchDefinition.Render()
        {
            throw new NotImplementedException();
        }
    }
}

