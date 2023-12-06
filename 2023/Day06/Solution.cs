using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day06;

[ProblemName("Wait For It")]
class Solution : Solver {

    public object PartOne(string input) {
        var races = Parse(input);
        var wins = races.Select(i => FindWaysToWin(i,GetPossibleButtonPress(i.Time)));
        return wins.Aggregate(1, (a,b) => a * b);
    }

    public object PartTwo(string input) {
        return 0;
    }

    record Race(int Time, int Distance);

    record ButtonPress
    {
        public int Speed;
        public int Duration;
        public int Distance;
        public ButtonPress(int speed, int duration)
        {
            Speed = speed;
            Duration = duration;
            Distance = speed * duration;
        }
    }
    
    IEnumerable<Race> Parse(string input)
    {
        var matches = Regex.Matches(input, @"\d+");
        var mid = matches.Count() / 2;
        var races = new List<Race>();
        for(var i = 0; i < mid; i++)
        {
            var time = int.Parse(matches.ElementAt(i).Value);
            var distance = int.Parse(matches.ElementAt(mid + i).Value);
            races.Add(new Race(time, distance));
        }
        return races;
    }

    IEnumerable<ButtonPress> GetPossibleButtonPress(int time)
    {
        var presses = new List<ButtonPress>();
        for(var i = 1; i < time; i++)
        {
            presses.Add(new (i, time - i));
        }
        return presses;
    }

    int FindWaysToWin(Race race, IEnumerable<ButtonPress> presses)
    {
        var wins = 0;
        foreach(var press in presses.Where(i => i.Speed < race.Time))
        {
            if(press.Distance > race.Distance)
            {
                Console.WriteLine($"Race: {race.Time}/{race.Distance} -> Press {press.Speed}/{press.Duration}");
                wins++;
            }
        }
        return wins;
    }
}
