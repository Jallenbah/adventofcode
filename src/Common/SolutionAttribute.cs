namespace AdventOfCode.Common;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal class SolutionAttribute : Attribute
{
    public SolutionAttribute(int year, int day)
    {
        Year = year;
        Day = day;
    }

    public int Year { get; set; }
    public int Day { get; set; }
}
