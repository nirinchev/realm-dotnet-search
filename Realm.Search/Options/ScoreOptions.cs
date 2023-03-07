using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Realm.Search;

public class AutocompleteScoreOptions
{
	public BoostOptions? BoostOptions { get; }

	public ConstantOptions? ConstantOptions { get; }

	public static AutocompleteScoreOptions Boost(float value)
		=> new(boost: new(value));

	public static AutocompleteScoreOptions Boost(string path, float? undefined = null)
		=> new(boost: new(path, undefined));

	public static AutocompleteScoreOptions Constant(float value)
		=> new(constant: new(value));

	private AutocompleteScoreOptions(BoostOptions? boost = null, ConstantOptions? constant = null)
	{
        BoostOptions = boost;
		ConstantOptions = constant;
	}

	internal BsonDocument Render()
	{
		var result = new BsonDocument();
		if (BoostOptions != null)
		{
			result["boost"] = BoostOptions.Render();
		}
        else if (ConstantOptions != null)
        {
            result["constant"] = ConstantOptions.Render();
        }
		else
		{
			throw new Exception("Unexpected AutocompleteScoreOptions value - no options have been set.");
		}

		return result;
    }
}

public class ScoreOptions
{
    public BoostOptions? BoostOptions { get; }

    public ConstantOptions? ConstantOptions { get; }

	public BsonDocument? FunctionOptions { get; }

    public static ScoreOptions Boost(float value)
    => new(boost: new(value));

    public static ScoreOptions Boost(string path, float? undefined = null)
        => new(boost: new(path, undefined));

    public static ScoreOptions Constant(float value)
        => new(constant: new(value));

    public static ScoreOptions Function(BsonDocument function)
		=> new(function: Argument.NotNull(function, nameof(function)));

	private ScoreOptions(BoostOptions? boost = null, ConstantOptions? constant = null, BsonDocument ? function = null)
	{
        BoostOptions = boost;
        ConstantOptions = constant;
        FunctionOptions = function;
	}

    internal BsonDocument Render()
    {
        var result = new BsonDocument();
        if (BoostOptions != null)
        {
            result["boost"] = BoostOptions.Render();
        }
        else if (ConstantOptions != null)
        {
            result["constant"] = ConstantOptions.Render();
        }
        else if (FunctionOptions != null)
        {
            result["function"] = FunctionOptions;
        }
        else
        {
            throw new Exception("Unexpected ScoreOptions value - no options have been set.");
        }

        return result;
    }
}

public class EmbeddedScoreOptions
{
    public BoostOptions? BoostOptions { get; }

    public ConstantOptions? ConstantOptions { get; }

    public BsonDocument? FunctionOptions { get; }

	public EmbeddedOptions? EmbeddedOptions { get; }

    public static EmbeddedScoreOptions Boost(float value)
		=> new(boost: new(value));

    public static EmbeddedScoreOptions Boost(string path, float? undefined = null)
        => new(boost: new(path, undefined));

    public static EmbeddedScoreOptions Constant(float value)
        => new(constant: new(value));

    public static EmbeddedScoreOptions Function(BsonDocument function)
        => new(function: Argument.NotNull(function, nameof(function)));

	public static EmbeddedScoreOptions Embedded(EmbeddedAggregateStrategy aggregate = EmbeddedAggregateStrategy.Sum, ScoreOptions? outerScope = null)
		=> new(embedded: new(outerScope, aggregate));

    private EmbeddedScoreOptions(BoostOptions? boost = null, ConstantOptions? constant = null, BsonDocument? function = null, EmbeddedOptions? embedded = null)
    {
        BoostOptions = boost;
        ConstantOptions = constant;
        FunctionOptions = function;
		EmbeddedOptions = embedded;
    }

    internal BsonDocument Render()
    {
        var result = new BsonDocument();
        if (BoostOptions != null)
        {
            result["boost"] = BoostOptions.Render();
        }
        else if (ConstantOptions != null)
        {
            result["constant"] = ConstantOptions.Render();
        }
        else if (FunctionOptions != null)
        {
            result["function"] = FunctionOptions;
        }
        else if(EmbeddedOptions != null)
        {
            result["embedded"] = EmbeddedOptions.Render();
        }
        else
        {
            throw new Exception("Unexpected AutocompleteScoreOptions value - no options have been set.");
        }

        return result;
    }
}

public class BoostOptions
{
	public float? Value { get; }

	public string? Path { get; }

	public float? Undefined { get; }

	internal BoostOptions(float value)
	{
        Value = value;
	}

	internal BoostOptions(string path, float? undefined)
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
            if (Undefined != null)
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

public class ConstantOptions
{
    public float Value { get; }

	internal ConstantOptions(float value)
	{
		Value = value;
	}

    internal BsonDocument Render() => new("value", Value);
}

public class EmbeddedOptions
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

public enum EmbeddedAggregateStrategy
{
	Sum,
	Maximum,
	Minimum,
	Mean,
}