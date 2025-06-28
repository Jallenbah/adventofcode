using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 6)]
internal class Day6 : ISolution
{
    public string Solve(string input)
    {
        var commandStrings = input.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var commands = commandStrings.Select(ReadCommandString).ToList();

        var part1 = Part1(commands);
        var part2 = Part2(commands);

        return
            $"Lights on in part 1: {part1}\n" +
            $"Total brightness in part 2: {part2}";
    }

    private Regex _commandRegex = new Regex(@"(?<cmd>turn on|turn off|toggle) (?<ax>\d+),(?<ay>\d+) through (?<bx>\d+),(?<by>\d+)");

    private record Point(int X, int Y);
    private record Command(Point A, Point B, CommandType CommandType);

    private enum CommandType
    {
        TurnOn,
        TurnOff,
        Toggle
    }

    private Command ReadCommandString(string command)
    {
        var match = _commandRegex.Match(command);

        if (!match.Success)
        {
            throw new InvalidInputException("Command in input does not match required pattern");
        }

        var commandType = match.Groups["cmd"].Value switch
        {
            "turn on" => CommandType.TurnOn,
            "turn off" => CommandType.TurnOff,
            "toggle" => CommandType.Toggle,
            _ => throw new NotImplementedException()
        };

        var ax = int.Parse(match.Groups["ax"].Value);
        var ay = int.Parse(match.Groups["ay"].Value);
        var bx = int.Parse(match.Groups["bx"].Value);
        var by = int.Parse(match.Groups["by"].Value);

        return new Command(new(ax, ay), new(bx, by), commandType);
    }

    private int Part1(List<Command> commands)
    {
        var lights = new bool[1000, 1000];

        foreach (var command in commands)
        {
            for (var x = command.A.X; x <= command.B.X; x++)
            for (var y = command.A.Y; y <= command.B.Y; y++)
            {
                try
                {
                    switch (command.CommandType)
                    {
                        case CommandType.TurnOn:
                            lights[x, y] = true;
                            break;
                        case CommandType.TurnOff:
                            lights[x, y] = false;
                            break;
                        case CommandType.Toggle:
                            lights[x, y] = !lights[x, y];
                            break;
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(
                        $"Tried to access out of range light {x}, {y}",
                        ex
                    );
                }
            }
        }

        var numLightsOn = 0;
        for (var x = 0; x < 1000; x++)
        for (var y = 0; y < 1000; y++)
        {
            numLightsOn += lights[x, y] ? 1 : 0;
        }

        return numLightsOn;
    }

    private int Part2(List<Command> commands)
    {
        var lights = new int[1000, 1000];

        foreach (var command in commands)
        {
            for (var x = command.A.X; x <= command.B.X; x++)
            for (var y = command.A.Y; y <= command.B.Y; y++)
            {
                try
                {
                    switch (command.CommandType)
                    {
                        case CommandType.TurnOn:
                            lights[x, y] += 1;
                            break;
                        case CommandType.TurnOff:
                            if (lights[x, y] > 0)
                            {
                                lights[x, y] -= 1;
                            }
                            break;
                        case CommandType.Toggle:
                            lights[x, y] += 2;
                            break;
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(
                        $"Tried to access out of range light {x}, {y}",
                        ex
                    );
                }
            }
        }

        var totalBrightness = 0;
        for (var x = 0; x < 1000; x++)
        for (var y = 0; y < 1000; y++)
        {
            totalBrightness += lights[x, y];
        }

        return totalBrightness;
    }
}
