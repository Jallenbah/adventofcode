using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 9)]
internal class Day9 : ISolution
{
    public string Solve(string input)
    {
        var distances = GetDistanceDictionary(input);

        var result = FindShortestPath(distances);

        return $"{result}";
    }

    private int FindShortestPath(Dictionary<string, Dictionary<string, int>> distances)
    {
        var locations = distances.Keys.ToList();

        return RecursivePathCheck(null, locations, 0);

        int RecursivePathCheck(string? previousLocation, List<string> unvistedLocations, int totalDistance)
        {
            if (unvistedLocations.Count == 0)
            {
                return totalDistance;
            }

            var shortestDistance = int.MaxValue;
            foreach (var thisLocation in unvistedLocations)
            {
                var locationsMinusThisOne = unvistedLocations.Where(l => l != thisLocation).ToList();
                var thisDistance = previousLocation == null ? 0 : distances[previousLocation][thisLocation];
                var pathDistance = thisDistance + RecursivePathCheck(thisLocation, locationsMinusThisOne, totalDistance);
                shortestDistance = pathDistance < shortestDistance ? pathDistance : shortestDistance;
            }
            return shortestDistance;

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
