# My advent of code solutions

Came to these puzzles late (June 2025), but I've started from the beginning and am working my way through them using C#.

If you want to use the framework I created for this that allows you to add and run the solutions easily then feel free to fork this repo and just delete all my solutions so you can add your own.

You register solutions with the solution attribute e.g.
```csharp
[Solution(2015, 1)]
internal class Day1 : ISolution
{
    public string Solve(string input)
    {
        return "hello world";
    }
}
```

This solution class will be picked up automatically when you run it if the command line parameters are set to year 2015, day 1. You can do that easily in Visual Studio by changing them in the `launchsettings.json` file:

```json
{
  "profiles": {
    "AdventOfCode": {
      "commandName": "Project",
      "commandLineArgs": "y=2015;d=1"
    }
  }
}
```

You will need to create the input .txt files as well. See [here](/src/inputs/README.md).