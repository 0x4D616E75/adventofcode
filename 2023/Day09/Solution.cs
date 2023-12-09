using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day09;

[ProblemName("Mirage Maintenance")]
class Solution : Solver {

    public object PartOne(string input) {
        var histories = Parse(input);
        var e = histories.Select(i => i.Extrapolate());
        return e.Sum();;
    }

    public object PartTwo(string input) {
        var histories = Parse(input);
        var e = histories.Select(i => i.ExtrapolateBackward());
        return e.Sum();;
    }

    IEnumerable<History> Parse(string input)
    {
        var histories = new List<History>();
        var lines = input.Split('\n');
        foreach(var line in lines)
        {
            var history = Regex.Matches(line, @"-?\d+");
            histories.Add(new History(history.Select(i => int.Parse(i.Value))));
        }
        return histories;
    }

    record History
    {
        public readonly IEnumerable<int> DataSet;
        
        public History(IEnumerable<int> dataSet)
        {
            DataSet = dataSet;
        }

        IEnumerable<int> GetDifferencesOf(IEnumerable<int> input)
        {
            var differences = new LinkedList<int>();
            for(var i = 0; i < input.Count() -1; i++)
            {
                differences.AddLast(input.ElementAt(i+1) - input.ElementAt(i));
            }
            return differences;
        }

        IEnumerable<IEnumerable<int>> GetAllDifferences()
        {
            var allDifferences = new LinkedList<IEnumerable<int>>();
            var differences = GetDifferencesOf(DataSet);
            allDifferences.AddLast(differences);
            while(!differences.All(i => i == 0))
            {
                differences = GetDifferencesOf(differences);
                allDifferences.AddLast(differences);
            }
            return allDifferences;
        }

        public int Extrapolate()
        {
            var allDifferences = GetAllDifferences();
            var lastDiffs = allDifferences.Select(i => i.Last()).Reverse();
            var last = lastDiffs.ElementAt(1) + lastDiffs.ElementAt(0);
            for(var i = 2; i < lastDiffs.Count(); i++ )
            {
                last = lastDiffs.ElementAt(i) + last;
            }

            return DataSet.Last() + last;
        }

        public int ExtrapolateBackward()
        {
            var allDifferences = GetAllDifferences();
            var firstDiff = allDifferences.Select(i => i.First()).Reverse();
            var beforeFirst = firstDiff.ElementAt(1) - firstDiff.ElementAt(0);
            for(var i = 2; i < firstDiff.Count(); i++ )
            {
                beforeFirst = firstDiff.ElementAt(i) - beforeFirst;
            }

            return DataSet.First() - beforeFirst;
        }
    }
}
