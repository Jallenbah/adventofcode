using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 8)]
internal class Day8 : ISolution
{
    public string Solve(string input)
    {
        var escapeRegex = new Regex(@"\\x[A-F0-9]{2}|\\[""\\]", RegexOptions.IgnoreCase);
        var slashAndQuoteRegex = new Regex(@"[\\""]");

        var strings = input.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var part1Result = strings.Select(Part1Check).Sum();
        var part2Result = strings.Select(Part2Check).Sum();

        return
            $"Part 1: {part1Result}\n" +
            $"Part 2: {part2Result}";

        // Add 2 to account for quotes without needing to trim them from the string.
        int Part1Check(string s) => s.Length - escapeRegex.Replace(s, "§").Length + 2;

        // If we escaped characters in slashAndQuoteRegex with an extra backslash, we'd just be adding the number of matches to the string's length.
        // Therefore no replacement is needed. We can just take the number of matches and add 2 to represent wrapping it with quotes.
        int Part2Check(string s) => slashAndQuoteRegex.Count(s) + 2;
    }
}
