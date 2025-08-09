using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 14)]
internal class Day14 : ISolution
{
    public string Solve(string input)
    {
        var reindeers = input.Split('\n')
            .Select(x => _inputStatementRegex.Match(x).Groups)
            .Select(x => new Reindeer(
                x["Name"].Value,
                int.Parse(x["Speed"].Value),
                int.Parse(x["FlyTime"].Value),
                int.Parse(x["RestTime"].Value)
            ))
            .ToList();

        for (var i = 0; i < 2503; i++)
        {
            foreach (var reindeer in reindeers)
            {
                reindeer.Progress1Second();
            }

            var leadingDistance = reindeers.Max(r => r.Distance);
            var reindeerInLead = reindeers.Where(r => r.Distance == leadingDistance);
            foreach (var reindeer in reindeerInLead)
            {
                reindeer.GivePoint();
            }
        }

        var reindeerWithFurthestDistance = reindeers.OrderByDescending(r => r.Distance).First();
        var reindeerWithMostPoints = reindeers.OrderByDescending(r => r.Points).First();

        return
            $"Part 1: {reindeerWithFurthestDistance.Distance}\n" +
            $"Part 2: {reindeerWithMostPoints.Points}";
    }

    private Regex _inputStatementRegex =
        new Regex(@"(?<Name>\w+) can fly (?<Speed>\d+) km\/s for (?<FlyTime>\d+) seconds, but then must rest for (?<RestTime>\d+) seconds.");

    private class Reindeer(string name, int speed, int flyTime, int restTime)
    {
        public int Distance { get; private set; } = 0;
        public int Points { get; private set; } = 0;

        private ReindeerState _state = ReindeerState.Moving;
        private int _secondsInCurrentState = 0;

        public void Progress1Second()
        {
            _secondsInCurrentState++;
            switch (_state)
            {
                case ReindeerState.Moving:
                    Distance += speed;
                    if (_secondsInCurrentState == flyTime)
                    {
                        _state = ReindeerState.Resting;
                        _secondsInCurrentState = 0;
                    }
                    break;
                case ReindeerState.Resting:
                    if (_secondsInCurrentState == restTime)
                    {
                        _state = ReindeerState.Moving;
                        _secondsInCurrentState = 0;
                    }
                    break;
            }
        }

        public void GivePoint() => Points++;
    }

    private enum ReindeerState
    {
        Moving,
        Resting
    }
}
