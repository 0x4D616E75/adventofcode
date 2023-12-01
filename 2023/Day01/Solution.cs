using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
class Solution : Solver {

    public object PartOne(string input) {
        var calibrationValues = ExtractCalibrationValues(input, regexPartOne);
        return calibrationValues.Sum();
    }

    public object PartTwo(string input) {
        var calibrationValues = ExtractCalibrationValues(input, regexPartTwo);
        return calibrationValues.Sum();
    }


    const string regexPartOne = @"\d";
    const string regexPartTwo = @"\d|one|two|three|four|five|six|seven|eight|nine";
    private IEnumerable<int> ExtractCalibrationValues(string input, string regexPattern)
    {
        var calibrationValues = new List<int>();
        var lines = input.Split('\n');
        foreach(var line in lines)
        {
            // For Part 2 the search need to be done from right to left to find digits as word when last and first letter are same.
            // e.g. oneight --> None finds "one" and RightToLeft finds "eight"
            var first = ParseToInt(Regex.Match(line, regexPattern, RegexOptions.None).Value);
            var last = ParseToInt(Regex.Match(line, regexPattern, RegexOptions.RightToLeft).Value);
            calibrationValues.Add(first * 10 + last);
        }
        return calibrationValues;
    }

    private int ParseToInt(string value)
    {
        switch (value)
        {
            case "one":
                return 1;
            case "two":
                return 2;
            case "three":
                return 3;
            case "four":
                return 4;
            case "five":
                return 5;
            case "six":
                return 6;
            case "seven":
                return 7;
            case "eight":
                return 8;
            case "nine":
                return 9;
            default:
                return int.Parse(value);
        }
        
    }
}
