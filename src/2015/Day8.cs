using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 8)]
internal class Day8 : ISolution
{
    public string Solve(string input)
    {
        var escapeRegex = new Regex(@"\\x[A-F0-9]{2}|\\[""\\]", RegexOptions.IgnoreCase);

        var strings = input.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var result = strings.Select(Check).Sum();
        return $"{result}";

        int Check(string s) => s.Length - escapeRegex.Replace(s, "§").Trim('"').Length;
    }
}
