using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 13)]
internal class Day13 : ISolution
{
    public string Solve(string input)
    {
        var inputLines = input.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var definitions = new List<Definition>(inputLines.Length);
        foreach (var line in inputLines)
        {
            var match = _parseRegex.Match(line);
            var subject = match.Groups["subject"].Value;
            var target = match.Groups["target"].Value;
            var amount = int.Parse(match.Groups["amount"].Value);
            amount *= match.Groups["operation"].Value == "lose" ? -1 : 1;
            definitions.Add(new(subject, target, amount));
        }

        return
            $"Part 1: {Part1(definitions)}\n" +
            $"Part 2: {Part2(definitions)}";
    }

    private int Part1(List<Definition> definitions)
    {
        var distinctPersons = definitions
            .Select(d => d.Subject)
            .Union(definitions.Select(d => d.Target))
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        var tableArrangements = GetPermutations(distinctPersons);

        var optimumHappinessArrangment = tableArrangements.Select(x => CalculateHappiness(x, definitions)).Max();

        return optimumHappinessArrangment;
    }

    private int Part2(List<Definition> definitions)
    {
        var distinctPersons = definitions
            .Select(d => d.Subject)
            .Union(definitions.Select(d => d.Target))
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        const string me = "Jallen";
        List<Definition> newDefinitions = [.. definitions];
        foreach(var person in distinctPersons)
        {
            newDefinitions.Add(new(me, person, 0));
            newDefinitions.Add(new(person, me, 0));
        }

        distinctPersons.Add(me);

        var tableArrangements = GetPermutations(distinctPersons);

        var optimumHappinessArrangment = tableArrangements.Select(x => CalculateHappiness(x, newDefinitions)).Max();

        return optimumHappinessArrangment;
    }

    // Gets only the permutations for a given first item in the list, rather than all possible permutations. This is because we are simulating a round
    // table, so to get all of the permutations would be getting the same table arrangement multiple times (it would be like just rotating the table
    // which would give each person the same position relative to the other people at the table)
    private List<List<string>> GetPermutations(List<string> list)
    {
        List<List<string>> BuildPermutationsRecursive(List<string> current, List<string> remaining)
        {
            if (!remaining.Any())
            {
                return [current];
            }

            var permutations = new List<List<string>>();
            foreach (var next in remaining)
            {
                List<string> newCurrent = [.. current, next];
                List<string> newRemaining = remaining.Where(x => x != next).ToList();

                permutations = [ ..permutations, ..BuildPermutationsRecursive(newCurrent, newRemaining) ];
            }

            return permutations;
        }

        return BuildPermutationsRecursive(list.Take(1).ToList(), list.Skip(1).ToList());
    }

    private int CalculateHappiness(List<string> tableArrangement, List<Definition> happinessDefinitions)
    {
        var happiness = 0;

        int GetNeighbourHappinessChange(string current, string neighbour) =>
            happinessDefinitions
                .Where(d => d.Subject == current && d.Target == neighbour)
                .Select(d => d.Value)
                .Single();

        var personIndex = 0;
        foreach(var person in tableArrangement)
        {
            // Find neighbours in a cyclical fashion - the first and last items are neighbours
            string neighbour1 = personIndex == 0
                ? tableArrangement.Last()
                : tableArrangement[personIndex - 1];
            string neighbour2 = personIndex == (tableArrangement.Count - 1)
                ? tableArrangement.First()
                : tableArrangement[personIndex + 1];

            happiness += GetNeighbourHappinessChange(person, neighbour1);
            happiness += GetNeighbourHappinessChange(person, neighbour2);

            personIndex++;
        }

        return happiness;
    }

    private Regex _parseRegex = new Regex(@"(?<subject>\w+) would (?<operation>gain|lose) (?<amount>\d+) happiness units by sitting next to (?<target>\w+).");
    record Definition(string Subject, string Target, int Value);
}
