using AdventOfCode.Common;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode._2015;

[Solution(2015, 4)]
internal class Day4 : ISolution
{
    public string Solve(string input)
    {
        var md5 = MD5.Create();

        int? startsWith5Zeros = null;
        int? startsWith6Zeros = null;

        const int maxLoopIterations = 1000 * 1000 * 1000;

        var i = 0;
        for (;; i++)
        {
            var testString = $"{input}{i}";

            var bytes = Encoding.UTF8.GetBytes(testString);
            var hash = md5.ComputeHash(bytes);
            var hashString = Convert.ToHexString(hash);

            if (hashString.StartsWith("00000"))
            {
                startsWith5Zeros ??= i;

                if (hashString.StartsWith("000000"))
                {
                    startsWith6Zeros ??= i;
                    break;
                }
            }

            // Just in case...
            if (i > maxLoopIterations)
            {
                return "Reached max loop iterations without finding solution.";
            }
        }

        return
            $"5 zeros: {startsWith5Zeros}\n" +
            $"6 zeros: {startsWith6Zeros}";
    }
}
