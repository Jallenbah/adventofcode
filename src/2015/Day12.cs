using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 12)]
internal class Day12 : ISolution
{
    public string Solve(string input)
    {
        var part1 = _numberRegex.Matches(input).Select(m => int.Parse(m.Value)).Sum();

        var part2 = DoPart2(input);

        return
            $"Part 1: {part1}\n" +
            $"Part 2: WIP"; //{part2}";
    }

    int DoPart2(string input)
    {
        return 0;
    }

    private Regex _numberRegex = new Regex(@"-?\d+");
}
