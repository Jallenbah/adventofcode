using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 7)]
internal class Day7 : ISolution
{
    public string Solve(string input)
    {
        var wireDefinitionStrings = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var wireDefinitions = wireDefinitionStrings.Select(GetWireDefinition).ToList();

        var wireValues = EmulateCircuit(wireDefinitions);

        var part1Solution = wireValues["a"];

        return $"Part 1: {part1Solution}";
    }

    private Regex _wireAndExpressionRegex = new Regex(@"(?<expr>.+) -> (?<wire>\w+)");
    private Regex _expressionRegex = new Regex(
        @"(?<valueOnly>^(?<value>\d+|\w+)$)|(?<notExpression>^NOT (?<notArg>\d+|\w+)$)|(?<binaryExpression>^(?<arg1>\d+|\w+) (?<gate>AND|OR|RSHIFT|LSHIFT) (?<arg2>\d+|\w+)$)");

    // We don't need WireDefinitionString here, but it makes debugging easier
    private record WireDefinition(string Wire, ExpressionType ExpressionType, string Arg1, string? Arg2, string WireDefinitionString);
    private enum ExpressionType { Value, Not, And, Or, Rshift, Lshift }

    // Takes a wire definition string (1 line from the input file) and turns it into an object containing the information we need to subsequently process it
    private WireDefinition GetWireDefinition(string wireDefinitionString)
    {
        var wireAndExpressionMatch = _wireAndExpressionRegex.Match(wireDefinitionString);
        if (!wireAndExpressionMatch.Success)
        {
            throw new InvalidInputException($"Wire definition does not match expected syntax {wireDefinitionString}");
        }

        var wireName = wireAndExpressionMatch.Groups["wire"].Value;
        var expression = wireAndExpressionMatch.Groups["expr"].Value;

        var expressionMatch = _expressionRegex.Match(expression);
        if (!wireAndExpressionMatch.Success)
        {
            throw new InvalidInputException($"Expression does not match expected syntax: {expression}");
        }

        if (expressionMatch.Groups["valueOnly"].Success)
        {
            return new WireDefinition(wireName, ExpressionType.Value, expressionMatch.Groups["value"].Value, null, wireDefinitionString);
        }
        else if (expressionMatch.Groups["notExpression"].Success)
        {
            return new WireDefinition(wireName, ExpressionType.Not, expressionMatch.Groups["notArg"].Value, null, wireDefinitionString);
        }
        else if (expressionMatch.Groups["binaryExpression"].Success)
        {
            var gateString = expressionMatch.Groups["gate"].Value;
            var gateType = gateString switch
            {
                "AND" => ExpressionType.And,
                "OR" => ExpressionType.Or,
                "RSHIFT" => ExpressionType.Rshift,
                "LSHIFT" => ExpressionType.Lshift,
                _ => throw new NotImplementedException()
            };

            return new WireDefinition(wireName, gateType, expressionMatch.Groups["arg1"].Value, expressionMatch.Groups["arg2"].Value, wireDefinitionString);
        }
        else
        {
            throw new Exception($"Could not match expression type for expression: {expression}");
        }
    }

    private Dictionary<string, ushort> EmulateCircuit(List<WireDefinition> wireDefinitions)
    {
        var results = new Dictionary<string, ushort>(wireDefinitions.Count);

        var previousUnresolvedDefinitionCount = 0;
        while (results.Count < wireDefinitions.Count)
        {
            var unresolvedDefinitions = wireDefinitions
                .Where(d => !results.Any(x => x.Key == d.Wire))
                .ToList();

            if (previousUnresolvedDefinitionCount == unresolvedDefinitions.Count)
            {
                throw new Exception("Cannot resolve all wire dependencies");
            }
            previousUnresolvedDefinitionCount = unresolvedDefinitions.Count;

            foreach (var definition in unresolvedDefinitions)
            {
                var resolvedWires = results.Keys.ToList();
                var dependencies = GetWireDefinitionDependencies(definition);
                var areDependenciesResolved = dependencies.All(resolvedWires.Contains);
                if (areDependenciesResolved)
                {
                    ushort wireValue = definition.ExpressionType switch
                    {
                        ExpressionType.Value => GetValue(definition.Arg1),
                        ExpressionType.Not => (ushort)~GetValue(definition.Arg1),
                        ExpressionType.And => (ushort)(GetValue(definition.Arg1) & GetValue(definition.Arg2!)),
                        ExpressionType.Or => (ushort)(GetValue(definition.Arg1) | GetValue(definition.Arg2!)),
                        ExpressionType.Rshift => (ushort)(GetValue(definition.Arg1) >> GetValue(definition.Arg2!)),
                        ExpressionType.Lshift => (ushort)(GetValue(definition.Arg1) << GetValue(definition.Arg2!)),
                        _ => throw new NotImplementedException()
                    };
                    results.Add(definition.Wire, wireValue);
                }
            }
        }

        return results;

        ushort GetValue(string arg) => ushort.TryParse(arg, out var val) ? val : results[arg];
    }

    private List<string> GetWireDefinitionDependencies(WireDefinition wireDefinition)
    {
        var dependencies = new List<string>(2);

        ProcessArg(wireDefinition.Arg1);
        if (wireDefinition.Arg2 != null)
        {
            ProcessArg(wireDefinition.Arg2);
        }

        return dependencies;

        void ProcessArg(string arg)
        {
            var isValueArg = ushort.TryParse(arg, out _);
            if (!isValueArg)
            {
                dependencies.Add(arg);
            }
        }
    }
}
