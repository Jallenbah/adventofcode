using AdventOfCode.Common;

namespace AdventOfCode._2015;

[Solution(2015, 3)]
internal class Day3 : ISolution
{
    public string Solve(string input)
    {
        // Part 1
        var soloSantaVisits = VisitHouses(input.ToList());
        var soloSantaHouseCount = soloSantaVisits.Count();

        // Part 2
        var listInitSize = (input.Length / 2) + 1; // Save having to do any dynamic resizes on add by allocating the whole thing up front
        var humanSantaDirections = new List<char>(listInitSize);
        var roboSantaDirections = new List<char>(listInitSize);
        for (var i = 0; i < input.Length; i++)
        {
            ((i % 2 == 0) ? humanSantaDirections : roboSantaDirections).Add(input[i]);
        }

        var humanSantaVisits = VisitHouses(humanSantaDirections);
        var roboSantaVisits = VisitHouses(roboSantaDirections);
        var distinctHouseCount = humanSantaVisits.Keys.Union(roboSantaVisits.Keys).Count();

        return
            $"Solo santa house count: {soloSantaHouseCount}\n" +
            $"Duo santa house count: {distinctHouseCount}";
    }

    private static Dictionary<Position, HouseDetails> VisitHouses(List<char> directions)
    {
        var currentPosition = new Position(0, 0);

        var housesDictionary = new Dictionary<Position, HouseDetails>()
        {
            { currentPosition, new HouseDetails(1) }
        };

        foreach (var token in directions)
        {
            switch (token)
            {
                case '<':
                    currentPosition = currentPosition with { X = currentPosition.X - 1 };
                    break;
                case '>':
                    currentPosition = currentPosition with { X = currentPosition.X + 1 };
                    break;
                case '^':
                    currentPosition = currentPosition with { Y = currentPosition.Y + 1 };
                    break;
                case 'v':
                    currentPosition = currentPosition with { Y = currentPosition.Y - 1 };
                    break;
                default:
                    throw new InvalidInputException($"Invalid character found in input: {token}");
            }

            if (housesDictionary.ContainsKey(currentPosition))
            {
                var currentHouse = housesDictionary[currentPosition];
                housesDictionary[currentPosition] = currentHouse with { VisitCount = currentHouse.VisitCount + 1 };
            }
            else
            {
                housesDictionary.Add(currentPosition, new HouseDetails(1));
            }
        }

        return housesDictionary;
    }

    public record Position(int X, int Y);
    public record HouseDetails(int VisitCount);
}
