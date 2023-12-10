using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day08;

[ProblemName("Haunted Wasteland")]
class Solution : Solver {

    public object PartOne(string input) {
        var (instructions, maps) = Parse(input, "AAA");
        return FindExit(instructions._instructions, maps.First(), "ZZZ");
    }

    public object PartTwo(string input) {
        var (instructions, maps) = Parse(input, "A");
        var exits = maps.Select(i => FindExit(instructions._instructions, i, "Z"));
        return exits.Sum();
    }

    public class Map
    {
        public readonly string Start;
        public Map Left;
        public Map Right;
        public Map(string start)
        {
            Start = start;
        }
        bool Contains(string start)
        {
            if( Start == start) return true;
            if(Left is not null && Left.Contains(start)) return true;
            if(Right is not null && Right.Contains(start)) return true;
            return false;
        }
    }
    
    public class Instructions
    {
        public readonly char[] _instructions;
        int _position = 0;
        readonly int _length;

        public Instructions(string value)
        {
            _instructions = value.ToCharArray();
            _length = _instructions.Length;
        }
       public char GetNextInstruction()
        {
            if(_position == _length)
            {
                _position = 0;
            }
            return _instructions[_position++];
        }
    }

    (Instructions, IEnumerable<Map>) Parse(string input, string start)
    {
        var lines = input.Split('\n');
        var instructions = new Instructions(lines[0]);
        var mapsTuple = lines.Skip(2).Select(i => Regex.Match(i, @"([A-Z]{3}) = \(([A-Z]{3}), ([A-Z]{3})\)")).Select(i => (i.Groups[1].Value, i.Groups[2].Value, i.Groups[3].Value));
        var maps = mapsTuple.Select(i => new Map(i.Item1)).ToArray();
        foreach(var map in maps)
        {
            var leftRight = mapsTuple.Where(i => i.Item1 == map.Start).Select(i => (i.Item2, i.Item3)).First();
            map.Left = maps.Where(i => i.Start == leftRight.Item1).First();
            map.Right = maps.Where(i => i.Start == leftRight.Item2).First();
        }

        return (instructions, maps.Where(i => i.Start.EndsWith(start)));
    }

    long FindExit(char[] instructions, Map map, string exit)
    {
        var steps = 0L;
        var next = map;
        do
        {
            next = instructions[steps % instructions.Length] == 'L' ? next.Left : next.Right;
            steps++;
        }
        while(!next.Start.EndsWith(exit));
        return steps;
    }
}
