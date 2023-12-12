using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day11;

[ProblemName("Cosmic Expansion")]
class Solution : Solver {

    public object PartOne(string input) {
        var map = Parse(input);
        var expandedMap = ExpandMap(map, 1);
        var galaxies = FindGalaxyPositions(expandedMap);
        var distances = GetCombinations(galaxies).Select(i => ManhattanDistance(i.Item1, i.Item2));
        return distances.Sum();
    }

    public object PartTwo(string input) {
        var map = Parse(input);
        var expandedMap = ExpandMap(map, 1000000);
        var galaxies = FindGalaxyPositions(expandedMap);
        var distances = GetCombinations(galaxies).Select(i => ManhattanDistance(i.Item1, i.Item2));
        return distances.Sum();
    }

    IEnumerable<IEnumerable<char>> Parse(string input)
    {
        var lines = input.Split('\n');
        var map = new char[lines.Count()][];
        for(var i = 0; i < lines.Count(); i++)
        {
            map[i] = lines[i].ToCharArray();
        }
        return map;
    }

    IEnumerable<IEnumerable<char>> ExpandMap(IEnumerable<IEnumerable<char>> map, int count)
    {
        return Transpose(FillExpandRow(Transpose(FillExpandRow(map, count)), count));
    }

    IEnumerable<IEnumerable<T>> Transpose<T>(IEnumerable<IEnumerable<T>> map)
    {
        var transposedMap = new T[map.First().Count()][];
        for(var i = 0; i < map.Count(); i++)
        {
            for(var k = 0; k < transposedMap.Length; k++)
            {
                if(transposedMap[k] == null)
                {
                    transposedMap[k] = new T[map.Count()];
                }
                var row = map.ElementAt(i);
                var cell = row.ElementAt(k);
                transposedMap[k][i] = cell;
            }
        }
        return transposedMap;
    }

    IEnumerable<IEnumerable<char>> FillExpandRow(IEnumerable<IEnumerable<char>> map, int count)
    {
        var expandedMap = new LinkedList<IEnumerable<char>>();
        var empty = Enumerable.Range(0, map.First().Count()).Select(_ => '.');
        for(var i = 0; i < map.Count(); i++)
        {
            if(map.ElementAt(i).All(i => i == '.'))
            {       
                for(var k = 0; k < count; k++)
                {
                    expandedMap.AddLast(empty);
                }
            }
            expandedMap.AddLast(map.ElementAt(i));
        }
        return expandedMap;
    }

    IEnumerable<(int,int)> FindGalaxyPositions(IEnumerable<IEnumerable<char>> map)
    {
        var galaxies = new List<(int, int)>();
        for(var i = 0; i < map.Count(); i++)
        {
            var row = map.ElementAt(i);
            for(var k = 0; k < row.Count(); k++)
            {
                if(row.ElementAt(k) == '#')
                {
                    galaxies.Add((i,k));
                }
            }
        }
        return galaxies;
    }

    int ManhattanDistance((int,int) a,(int, int) b) => Math.Abs(b.Item1 - a.Item1) + Math.Abs(b.Item2 - a.Item2);

    IEnumerable<((int,int),(int,int))> GetCombinations(IEnumerable<(int,int)> input)
    {
        var combinations = new List<((int,int),(int,int))>();
        var length = input.Count();
        for(var i = 0; i < length; i++)
        {
            for(var k = i + 1; k < length; k++)
            {
                combinations.Add((input.ElementAt(i), input.ElementAt(k)));
            }
        }
        return combinations;
    }
}

