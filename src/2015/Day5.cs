using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 5)]
internal class Day5 : ISolution
{
    public string Solve(string input)
    {
        var strings = input.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var part1NiceCount = 0;
        var part2NiceCount = 0;

        foreach(var str in strings)
        {
            part1NiceCount += Part1IsNice(str) ? 1 : 0;
            part2NiceCount += Part2IsNice(str) ? 1 : 0;
        }

        return
            "Nice Strings ~\n" +
            $"Part 1: {part1NiceCount}\n" +
            $"Part 2: {part2NiceCount}";
    }

    // Part 1 regex
    private Regex _vowelRegex = new Regex(@"[aeiou]");
    private Regex _doubleCharRegex = new Regex(@"(\w)\1");
    private Regex _forbiddenRegex = new Regex(@"ab|cd|pq|xy");

    private bool Part1IsNice(string str)
    {
        var has3Vowels = _vowelRegex.Count(str) >= 3;
        var hasDoubleChar = _doubleCharRegex.IsMatch(str);
        var hasForbiddenStuff = _forbiddenRegex.IsMatch(str);

        return (has3Vowels && hasDoubleChar && !hasForbiddenStuff);
    }

    // Part 2 regex
    private Regex _xyxRegex = new Regex(@"(\w)\w\1");
    private Regex _doublePairRegex = new Regex(@"(\w\w).*\1");

    private bool Part2IsNice(string str)
    {
        var hasXyx = _xyxRegex.IsMatch(str);
        var hasDoublePair = _doublePairRegex.IsMatch(str);

        return hasXyx && hasDoublePair;
    }
}
