using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 9)]
internal class Day9 : ISolution
{
    public string Solve(string input)
    {
        var distances = GetDistanceDictionary(input);

        var result = FindShortestAndLongestPaths(distances);

        return $"{result}";
    }

    record ShortestLongest(int Shortest, int Longest);
    private ShortestLongest FindShortestAndLongestPaths(Dictionary<string, Dictionary<string, int>> distances)
    {
        var locations = distances.Keys.ToList();

        return RecursivePathCheck(null, locations, new ShortestLongest(0, 0));

        ShortestLongest RecursivePathCheck(string? thisLocation, List<string> unvistedLocations, ShortestLongest totalDistance)
        {
            if (unvistedLocations.Count == 0)
            {
                return totalDistance;
            }

            var shortestDistance = int.MaxValue;
            var longestDistance = 0;
            foreach (var nextLocation in unvistedLocations)
            {
                var locationsExceptThisOne = unvistedLocations.Where(l => l != nextLocation).ToList();
                var distanceToNextLocation = thisLocation == null ? 0 : distances[thisLocation][nextLocation];
                var pathDistances = RecursivePathCheck(nextLocation, locationsExceptThisOne, totalDistance);

                var shortestPath = distanceToNextLocation + pathDistances.Shortest;
                var longestPath = distanceToNextLocation + pathDistances.Longest;

                shortestDistance = shortestPath < shortestDistance ? shortestPath : shortestDistance;
                longestDistance = longestPath > longestDistance ? longestPath : longestDistance;
            }
            return new ShortestLongest(shortestDistance, longestDistance);

        }
    }

    private Dictionary<string, Dictionary<string, int>> GetDistanceDictionary(string input)
    {
        var distanceDefinitionStrings = input.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var distanceDefinitionRegex = new Regex(@"(?<LocationA>\w+) to (?<LocationB>\w+) = (?<Distance>\d+)");

        var distanceDefinitions = distanceDefinitionStrings
            .Select(x => distanceDefinitionRegex.Match(x).Groups)
            .Select(x => new
            {
                LocationA = x["LocationA"].Value,
                LocationB = x["LocationB"].Value,
                Distance = int.Parse(x["Distance"].Value)
            })
            .ToList();

        var uniqueLocations = distanceDefinitions
            .Select(d => d.LocationA)
            .Union(distanceDefinitions.Select(d => d.LocationB))
            .Distinct()
            .ToList();

        int GetDistance(string locationA, string locationB) =>
            (
                distanceDefinitions.FirstOrDefault(d => d.LocationA == locationA && d.LocationB == locationB) ??
                distanceDefinitions.FirstOrDefault(d => d.LocationA == locationB && d.LocationB == locationA)
            )!.Distance;

        var distanceDictionary = new Dictionary<string, Dictionary<string, int>>(uniqueLocations.Count);
        foreach (var location in uniqueLocations)
        {
            var destinationDistances = new Dictionary<string, int>(uniqueLocations.Count);
            foreach (var destination in uniqueLocations.Where(l => l != location)) // No point in getting distance to itself, so exclude the starting location
            {
                destinationDistances.Add(destination, GetDistance(location, destination));
            }
            distanceDictionary.Add(location, destinationDistances);
        }

        return distanceDictionary;
    }
}
