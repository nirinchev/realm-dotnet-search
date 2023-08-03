using System;
using System.Linq;
using MongoDB.Bson;

namespace Realms.Search
{
    /// <summary>
    /// A class representing a path or a collection of paths in the document.
    /// </summary>
    public class PathDefinition
    {
        internal BsonValue Value { get; }

        private PathDefinition(BsonValue value)
        {
            Value = value;
        }

        /// <summary>
        /// An operator that converts a single path to a <see cref="PathDefinition"/>.
        /// </summary>
        /// <param name="path">The path in the document that Atlas will search for.</param>
        public static implicit operator PathDefinition(string path)
            => new(new BsonString(path));

        /// <summary>
        /// An operator that converts an array of paths to a <see cref="PathDefinition"/>.
        /// </summary>
        /// <param name="paths">
        /// The paths in the document that Atlas will search for. Documents which match on any of the
        /// specified fields are included in the result set.
        /// </param>
        public static implicit operator PathDefinition(string[] paths)
            => new(new BsonArray(paths.Select(t => new BsonString(t))));
    }
}
