using AdventOfCode.Common;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode._2015;

[Solution(2015, 4)]
internal class Day4 : ISolution
{
    public string Solve(string input)
    {
        // Single-threaded solution is simpler, but multi-threaded solution is 4-5x faster on my hardware.
        var solutionChoice = SolutionChoice.MultiThreaded;

        var before = Stopwatch.GetTimestamp();

        string result = solutionChoice switch
        {
            SolutionChoice.SingleThreaded => SingleThreadedImplementation(input),
            SolutionChoice.MultiThreaded => MultiThreadedImplementation(input),
            _ => throw new NotImplementedException()
        };

        var after = Stopwatch.GetTimestamp();
        var delta = TimeSpan.FromTicks(after - before);

        return
            $"{result}\n" +
            $"({delta.TotalMilliseconds:#.#}ms)";
    }

    private enum SolutionChoice { SingleThreaded, MultiThreaded }

    private string SingleThreadedImplementation(string input)
    {
        int? startsWith5Zeros = null;
        int? startsWith6Zeros = null;

        const int maxLoopIterations = 1000 * 1000 * 1000;

        var i = 0;
        for (; ; i++)
        {
            var testString = $"{input}{i}";

            var bytes = Encoding.UTF8.GetBytes(testString);
            var hash = MD5.HashData(bytes);
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

    private string MultiThreadedImplementation(string input)
    {
        int? startsWith5Zeros = null;
        int? startsWith6Zeros = null;

        const int maxLoopIterations = 1000 * 1000 * 1000;
        const int parallelChunkSize = 100 * 1000;

        var lockObject = new object();

        for (var x = 0; x < maxLoopIterations; x += parallelChunkSize)
        {
            Parallel.For(x, x + parallelChunkSize, (i, loopState) =>
            {
                var testString = $"{input}{i}";

                var bytes = Encoding.UTF8.GetBytes(testString);
                var hash = MD5.HashData(bytes);
                var hashString = Convert.ToHexString(hash);

                if (hashString.StartsWith("00000"))
                {
                    lock (lockObject)
                    {
                        if (startsWith5Zeros == null || startsWith5Zeros.Value > i)
                        {
                            startsWith5Zeros = i;
                        }

                        if (hashString.StartsWith("000000"))
                        {
                            if (startsWith6Zeros == null || startsWith6Zeros.Value > i)
                            {
                                startsWith6Zeros = i;
                            }
                        }
                    }
                }
            });

            if (startsWith5Zeros != null && startsWith6Zeros != null)
            {
                break;
            }
        }

        return
            $"5 zeros: {startsWith5Zeros}\n" +
            $"6 zeros: {startsWith6Zeros}";
    }
}
