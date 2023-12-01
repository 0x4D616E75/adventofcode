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
        var calibrationValues = ExtractCalibrationValues(input);
        return calibrationValues.Sum();
    }

    public object PartTwo(string input) {
        return 0;
    }
    Regex regex = new (@"\d");
    private IEnumerable<int> ExtractCalibrationValues(string input)
    {
        var calibrationValues = new List<int>();
        var lines = input.Split('\n');
        foreach(var line in lines)
        {
            var matches = regex.Matches(line);
            var first = int.Parse(matches.FirstOrDefault().Value);
            var last = int.Parse(matches.LastOrDefault().Value);
            calibrationValues.Add(int.Parse($"{first}{last}"));
        }
        return calibrationValues;
    }
}
