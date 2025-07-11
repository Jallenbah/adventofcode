using AdventOfCode.Common;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 12)]
internal class Day12 : ISolution
{
    public string Solve(string input)
    {
        var part1 = TotalNumbersInJsonString(input);

        var part2 = part1 - GetRedTotal(input);

        return
            $"Part 1: {part1}\n" +
            $"Part 2: {part2}";
    }

    private Regex _numberRegex = new Regex(@"-?\d+");

    private int TotalNumbersInJsonString(string json) => _numberRegex.Matches(json).Sum(m => int.Parse(m.Value));

    private int GetRedTotal(string input)
    {
        return ProcessNodeRecursive(JsonNode.Parse(input)!);

        int ProcessNodeRecursive(JsonNode node)
        {
            if (node is JsonObject obj)
            {
                if (obj.Any(p =>
                    p.Value is JsonValue &&
                    p.Value.GetValueKind() == System.Text.Json.JsonValueKind.String &&
                    p.Value.GetValue<string>() == "red"))
                {
                    // We could recursively go through and actually pick out the values, but it's actually much
                    // simpler to just treat this particlar object as a JSON string like we did in part 1.
                    return TotalNumbersInJsonString(obj.ToJsonString());
                }
                else
                {
                    return ProcessNodeCollection(obj.Select(o => o.Value)!);
                }
            }
            else if (node is JsonArray arr)
            {
                return ProcessNodeCollection(arr!);
            }
            else
            {
                return 0;
            }
        }

        int ProcessNodeCollection(IEnumerable<JsonNode> nodes) => nodes.Sum(ProcessNodeRecursive);
    }

    
}
