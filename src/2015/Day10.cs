using AdventOfCode.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 10)]
internal class Day10 : ISolution
{
    public string Solve(string input)
    {
        var p1 = EvaluateTimes(input, 40);

        var p2 = EvaluateTimes(input, 50);

        return
            $"Part 1: {p1.Length}\n" +
            $"Part 2: {p2.Length}";
    }

    string EvaluateTimes(string input, int times)
    {
        var str = input;
        for (var i = 0; i < times; i++)
        {
            str = Evaluate(str);
        }
        return str;
    }

    private Regex _digitRegex = new Regex(@"(\d)\1*", RegexOptions.Compiled);

    string Evaluate(string input)
    {
        var matches = _digitRegex.Matches(input);

        StringBuilder b = new StringBuilder();
        foreach (var match in matches)
        {
            var matchString = match.ToString()!;
            var replacement = $"{matchString.Length}{matchString[0]}";
            b.Append(replacement);
        }

        return b.ToString();
    }
}
