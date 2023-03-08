using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Realms.Search;

/// <summary>
/// Score options for a <see cref="AutocompleteDefinition"/> search.
/// </summary>
/// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/"/>
public class ScoreOptions
{
    private readonly BoostOptions? _boost;

    private readonly ConstantOptions? _constant;

    /// <summary>
    /// Constructs a boost option that multiplies the score by the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value by which the score will be multiplied.</param>
    /// <returns>An <see cref="ScoreOptions"/> instance that will multiply the search score.</returns>
    /// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/#boost"/>
	public static ScoreOptions Boost(float value)
        => new(boost: new(value));

    /// <summary>
    /// Constructs a boost option that multiplies the score by the value contained in a field of the document.
    /// </summary>
    /// <param name="path">Name of the numeric field whose value to multiply the default base score by.</param>
    /// <param name="undefined">Numeric value to substitute for path if the numeric field specified through path is not found in the documents. If omitted, defaults to 0.</param>
    /// <returns>An <see cref="ScoreOptions"/> instance that will multiply the search score.</returns>
    /// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/#boost"/>
	public static ScoreOptions Boost(string path, float undefined = 0)
        => new(boost: new(path, undefined));

    /// <summary>
    /// Constructs a constant option that replaces the base score with the specified number.
    /// </summary>
    /// <param name="value">The value that replaces the base score.</param>
    /// <returns>An <see cref="ScoreOptions"/> instance that replaces the search score by a constant.</returns>
    /// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/#constant"/>
	public static ScoreOptions Constant(float value)
        => new(constant: new(value));

    private ScoreOptions(BoostOptions? boost = null, ConstantOptions? constant = null)
    {
        _boost = boost;
        _constant = constant;
    }

    internal BsonDocument Render()
    {
        var result = new BsonDocument();
        if (_boost != null)
        {
            result["boost"] = _boost.Render();
        }
        else if (_constant != null)
        {
            result["constant"] = _constant.Render();
        }
        else
        {
            throw new Exception("Unexpected ScoreOptions value - no options have been set.");
        }

        return result;
    }
}

/// <summary>
/// Score options for an <see cref="EmbeddedDocumentDefinition"/> search.
/// </summary>
/// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/"/>
public class EmbeddedScoreOptions
{
    private readonly BoostOptions? _boost;

    private readonly ConstantOptions? _constant;

    private readonly BsonDocument? _function;

    private readonly EmbeddedOptions? _embedded;

    /// <summary>
    /// Constructs a boost option that multiplies the score by the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value by which the score will be multiplied.</param>
    /// <returns>An <see cref="EmbeddedScoreOptions"/> instance that will multiply the search score.</returns>
    /// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/#boost"/>
    public static EmbeddedScoreOptions Boost(float value)
        => new(boost: new(value));

    /// <summary>
    /// Constructs a boost option that multiplies the score by the value contained in a field of the document.
    /// </summary>
    /// <param name="path">Name of the numeric field whose value to multiply the default base score by.</param>
    /// <param name="undefined">Numeric value to substitute for path if the numeric field specified through path is not found in the documents. If omitted, defaults to 0.</param>
    /// <returns>An <see cref="EmbeddedScoreOptions"/> instance that will multiply the search score.</returns>
    /// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/#boost"/>
	public static EmbeddedScoreOptions Boost(string path, float undefined = 0)
        => new(boost: new(path, undefined));

    /// <summary>
    /// Constructs a constant option that replaces the base score with the specified number.
    /// </summary>
    /// <param name="value">The value that replaces the base score.</param>
    /// <returns>An <see cref="EmbeddedScoreOptions"/> instance that replaces the search score with a constant.</returns>
    /// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/#constant"/>
	public static EmbeddedScoreOptions Constant(float value)
        => new(constant: new(value));

    /// <summary>
    /// Constructs a function option that allows you to alter the base score.
    /// </summary>
    /// <param name="function">
    /// The function option allows you to alter the final score of the document using a numeric field.
    /// You can specify the numeric field for computing the final score through an expression.If the final
    /// result of the function score is less than 0, Atlas Search replaces the score with 0.
    /// </param>
    /// <returns>An <see cref="EmbeddedScoreOptions"/> instance that replaces the search score with the result of a function call.</returns>
    /// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/#function"/>
    public static EmbeddedScoreOptions Function(BsonDocument function)
        => new(function: Argument.NotNull(function, nameof(function)));

    /// <summary>
    /// Constructs an embedded option that allows you to aggregate the scores of multiple matching embedded documents.
    /// </summary>
    /// <param name="aggregate">Configures how to combine scores of matching embedded documents. Value must be one of the following aggregation strategies.</param>
    /// <param name="outerScope">Specifies the score modification to apply after applying the aggregation strategy.</param>
    /// <returns>An <see cref="EmbeddedScoreOptions"/> instance that replaces the search score with the result of an aggregation across multiple documents.</returns>
    /// <seealso href="https://www.mongodb.com/docs/atlas/atlas-search/scoring/#embedded"/>
    public static EmbeddedScoreOptions Embedded(EmbeddedAggregateStrategy aggregate = EmbeddedAggregateStrategy.Sum, ScoreOptions? outerScope = null)
        => new(embedded: new(outerScope, aggregate));

    private EmbeddedScoreOptions(BoostOptions? boost = null, ConstantOptions? constant = null, BsonDocument? function = null, EmbeddedOptions? embedded = null)
    {
        _boost = boost;
        _constant = constant;
        _function = function;
        _embedded = embedded;
    }

    internal BsonDocument Render()
    {
        var result = new BsonDocument();
        if (_boost != null)
        {
            result["boost"] = _boost.Render();
        }
        else if (_constant != null)
        {
            result["constant"] = _constant.Render();
        }
        else if (_function != null)
        {
            result["function"] = _function;
        }
        else if (_embedded != null)
        {
            result["embedded"] = _embedded.Render();
        }
        else
        {
            throw new Exception("Unexpected EmbeddedScoreOptions value - no options have been set.");
        }

        return result;
    }
}

internal class BoostOptions
{
    public float? Value { get; }

    public string? Path { get; }

    public float Undefined { get; }

    internal BoostOptions(float value)
    {
        Value = value;
    }

    internal BoostOptions(string path, float undefined)
    {
        Path = Argument.NotNull(path, nameof(path));
        Undefined = undefined;
    }

    internal BsonDocument Render()
    {
        var result = new BsonDocument();
        if (Value != null)
        {
            result["value"] = Value;
        }
        else if (Path != null)
        {
            result["path"] = Path;
            if (Undefined != 0)
            {
                result["undefined"] = Undefined;
            }
        }
        else
        {
            throw new Exception("Unexpected BoostOptions - no options have been set.");
        }

        return result;
    }
}

internal class ConstantOptions
{
    public float Value { get; }

    internal ConstantOptions(float value)
    {
        Value = value;
    }

    internal BsonDocument Render() => new("value", Value);
}

internal class EmbeddedOptions
{
    public EmbeddedAggregateStrategy Aggregate { get; }

    public ScoreOptions? OuterScope { get; }

    internal EmbeddedOptions(ScoreOptions? outerScope, EmbeddedAggregateStrategy aggregate)
    {
        OuterScope = outerScope;
        Aggregate = aggregate;
    }

    internal BsonDocument Render()
    {
        var result = new BsonDocument();
        if (Aggregate != EmbeddedAggregateStrategy.Sum)
        {
            result["aggregate"] = Aggregate.ToString().ToLowerInvariant();
        }

        if (OuterScope != null)
        {
            result["outerScope"] = OuterScope.Render();
        }

        return result;
    }
}

/// <summary>
/// The strategy controlling how to combine scores of matching embedded documents.
/// </summary>
public enum EmbeddedAggregateStrategy
{
    /// <summary>
    /// Sum the score of all matching embedded documents.
    /// </summary>
	Sum,

    /// <summary>
    /// Choose the greatest score of all matching embedded documents.
    /// </summary>
	Maximum,

    /// <summary>
    /// Choose the least high score of all matching embedded documents.
    /// </summary>
	Minimum,

    /// <summary>
    /// Choose the average (arithmetic mean) score of all matching embedded documents.
    /// Atlas Search include scores of matching embedded documents only when computing the average.
    /// Atlas Search doesn't count embedded documents that don't satisfy query predicates as documents
    /// with scores of zero.
    /// </summary>
	Mean,
}