using AdventOfCode.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

[Solution(2015, 11)]
internal class Day11 : ISolution
{
    private Regex _consecutiveCharsRegex = ConstructConsecutiveCharsRegex();
    private Regex _forbiddenCharsRegex = new Regex(@"i|o|l");
    private Regex _twoDifferentPairsRegex = new Regex(@"(?<a>(?<x>\w)\k<x>)(?!\w*\k<a>)");

    public string Solve(string input)
    {
        var part1Solution = FindNextPassword(input);
        var part2Solution = FindNextPassword(part1Solution);

        return
            $"Part 1: {part1Solution}\n" +
            $"Part 2: {part2Solution}";
    }

    private string FindNextPassword(string initialPassword)
    {
        var currentPassword = IncrementPassword(initialPassword);
        while (!DoesPasswordMeetRules(currentPassword))
        {
            currentPassword = IncrementPassword(currentPassword);
            if (currentPassword == initialPassword)
            {
                throw new Exception("We've tried every string and none of them meet the requirements...");
            }
        }
        return currentPassword;
    }

    private bool DoesPasswordMeetRules(string password)
    {
        return _consecutiveCharsRegex.IsMatch(password) &&
               !_forbiddenCharsRegex.IsMatch(password) &&
               _twoDifferentPairsRegex.Matches(password).Count > 1;
    }

    private static string IncrementPassword(string password)
    {
        var passwordArray = password.ToCharArray();
        for (var i = passwordArray.Length - 1; i >= 0; i--)
        {
            if (passwordArray[i] == 'z')
            {
                passwordArray[i] = 'a';
                continue;
            }
            passwordArray[i]++;
            break;
        }

        return new string(passwordArray);
    }

    private static Regex ConstructConsecutiveCharsRegex()
    {
        var consecutiveCharsRegexStringBuilder = new StringBuilder();
        var chars = new char[3] { 'a', 'b', 'c' };
        consecutiveCharsRegexStringBuilder.Append(chars);
        while (chars[chars.Length - 1] != 'z')
        {
            for (var i = 0; i < chars.Length; i++) chars[i]++;
            consecutiveCharsRegexStringBuilder.Append('|');
            consecutiveCharsRegexStringBuilder.Append(chars);
        }
        var regexString = consecutiveCharsRegexStringBuilder.ToString();
        return new Regex(regexString);
    }
}
