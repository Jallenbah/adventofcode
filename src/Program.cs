using AdventOfCode.Common;
using System.Text.RegularExpressions;

var argsRegex = new Regex("^(\\w+=[^;]+;*)*$");
if (args.Length != 1 || !argsRegex.IsMatch(args[0]))
{
    WriteErrorAndWait(
        "Command line arguments missing or badly formatted.\n" +
        "Specify the year and day of the puzzle as a command line argument in format 'y=####;d=#' e.g. y=2015;d=1");
    return;
}

// Gets a collection of arrays e.g. [["y", "2015"], ["d", "1"]]
var splitArgs = args[0]
    .Split(';')
    .Select(a => a.Split("=", 2));

// Turn into a dictionary keyed by arg name
var argumentDictionary = new Dictionary<string, string>(
    splitArgs.Select(x => new KeyValuePair<string, string>(x[0], x[1]))
);

var year = argumentDictionary["y"];
var day = argumentDictionary["d"];

var inputFilePath = $"inputs/{year}/{day}.txt";

if (!File.Exists(inputFilePath))
{
    WriteErrorAndWait($"The input file for year {year} day {day} does not exist.");
    return;
}

ISolution? solutionInstance;
try
{
    solutionInstance = SolutionFactory.Get(year, day);
}
catch (Exception ex)
{
    WriteErrorAndWait(
        $"Error obtaining solution instance for year {year} day {day}.\n" +
        $"{ex}");
    return;
}

if (solutionInstance == null)
{
    WriteErrorAndWait($"Could not find solution instance for year {year} day {day}.");
    return;
}

var inputFileContents = File.ReadAllText(inputFilePath);

if (string.IsNullOrWhiteSpace(inputFileContents))
{
    WriteErrorAndWait($"Input file empty for year {year} day {day}.");
    return;
}

string solution;
try
{
    solution = solutionInstance.Solve(inputFileContents);
}
catch (Exception ex)
{
    WriteErrorAndWait(
        $"Error in solution code for year {year} day {day}.\n" +
        $"{ex}");
    return;
}

Console.WriteLine(solution);
Console.ReadLine();

void WriteErrorAndWait(string error) {
    Console.WriteLine(error);
    Console.ReadLine();
}
