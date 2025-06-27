using AdventOfCode.Common;

namespace AdventOfCode._2015;

[Solution(2015, 1)]
internal class Day1 : ISolution
{
    public string Solve(string input)
    {
        int floor = 0;
        int? firstBasementEntryPosition = null;

        for(int i = 0; i < input.Length; i++)
        {
            var token = input[i];
            switch (token)
            {
                case '(':
                    floor++;
                    break;
                case ')':
                    floor--;
                    break;
                default:
                    throw new InvalidInputException($"Unexpected character encountered: {token}");
            }

            if (floor < 0 && firstBasementEntryPosition == null)
            {
                firstBasementEntryPosition = i + 1; // +1 because index starts at 0 and positions start at 1
            }
        }

        return
            $"Floor: {floor}\n" +
            $"First basement entry position: {firstBasementEntryPosition}";
    }
}
