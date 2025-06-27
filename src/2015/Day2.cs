using AdventOfCode.Common;

namespace AdventOfCode._2015;

[Solution(2015, 2)]
internal class Day2 : ISolution
{
    public string Solve(string input)
    {
        var presents = input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split('x').Select(d => d.Trim()).ToArray())
            .Select(x => new Present(
                    int.Parse(x[0]),
                    int.Parse(x[1]),
                    int.Parse(x[2])
                ))
            .ToList();

        int totalPaperAreaRequired = 0;
        int totalRibbonRequired = 0;
        foreach(var present in presents)
        {
            totalPaperAreaRequired += CalculateRequiredPaperArea(present);
            totalRibbonRequired += CalculateRequiredRibbon(present);
        }

        return
            $"Paper area required: {totalPaperAreaRequired}\n" +
            $"Ribbon required: {totalRibbonRequired}";
    }

    private int CalculateRequiredPaperArea(Present present)
    {
        int[] sideAreas = [
            present.Width * present.Height, // front
            present.Length * present.Height, // side
            present.Width * present.Length // top
        ];

        var smallestSideArea = sideAreas.Min();

        var totalSurfaceArea = sideAreas.Sum() * 2;

        var totalSurfaceAreaPlusExtra = totalSurfaceArea + smallestSideArea;

        return totalSurfaceAreaPlusExtra;
    }

    private int CalculateRequiredRibbon(Present present)
    {
        int[] sidePerimeters = [
            (2 * present.Width) + (2 * present.Height), // front
            (2 * present.Length) + (2 * present.Height), // side
            (2 * present.Width) + (2 * present.Length) // top
        ];

        var smallestPerimeter = sidePerimeters.Min();

        var volumeForBow = present.Length * present.Width * present.Height;

        var ribbonRequired = smallestPerimeter + volumeForBow;

        return ribbonRequired;
    }

    internal record struct Present(int Length, int Width, int Height);
}
