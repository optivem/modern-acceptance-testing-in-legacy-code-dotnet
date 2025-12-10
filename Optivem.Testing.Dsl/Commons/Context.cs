using System.Collections.Generic;

namespace Optivem.EShop.SystemTest.Core.Dsl.Commons;

public class Context
{
    private readonly Dictionary<string, string> _paramMap = new();
    private readonly Dictionary<string, string> _resultMap = new();

    public string GetParamValue(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            return alias;
        }

        if (_paramMap.TryGetValue(alias, out var value))
        {
            return value;
        }

        var generatedValue = GenerateParamValue(alias);
        _paramMap[alias] = generatedValue;

        return generatedValue;
    }

    public void SetResultEntry(string alias, string value)
    {
        if (_resultMap.ContainsKey(alias))
        {
            throw new InvalidOperationException($"Alias already exists: {alias}");
        }

        _resultMap[alias] = value;
    }

    public string GetResultValue(string alias)
    {
        if (_resultMap.TryGetValue(alias, out var value))
        {
            return value;
        }

        return alias; // Return literal value if not found as alias
    }

    public string ExpandAliases(string message)
    {
        var expandedMessage = ExpandAlias(message, _paramMap);
        expandedMessage = ExpandAlias(expandedMessage, _resultMap);
        return expandedMessage;
    }

    private static string ExpandAlias(string message, Dictionary<string, string> map)
    {
        var expandedMessage = message;
        foreach (var entry in map)
        {
            var alias = entry.Key;
            var actualValue = entry.Value;
            expandedMessage = expandedMessage.Replace(alias, actualValue);
        }
        return expandedMessage;
    }

    private static string GenerateParamValue(string alias)
    {
        var suffix = Guid.NewGuid().ToString()[..8];
        return $"{alias}-{suffix}";
    }
}
