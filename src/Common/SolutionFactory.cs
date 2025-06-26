using System.Reflection;

namespace AdventOfCode.Common;

internal static class SolutionFactory
{
    public static ISolution? Get(string year, string day)
    {
        // Reflection is an appropriate solution for this problem.
        // This simplifies the process of adding new solutions greatly without the need to centrally register them anywhere.
        var assembly = Assembly.GetExecutingAssembly();
        foreach (Type type in assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(ISolution).IsAssignableFrom(t)))
        {
            var solutionAttribute = type.GetCustomAttribute<SolutionAttribute>(false);
            if (solutionAttribute != null &&
                solutionAttribute.Year.ToString() == year &&
                solutionAttribute.Day.ToString() == day)
            {
                return Activator.CreateInstance(type) as ISolution;
            }
        }

        return null;
    }
}
