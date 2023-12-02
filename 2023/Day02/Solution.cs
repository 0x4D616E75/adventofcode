using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day02;

[ProblemName("Cube Conundrum")]
class Solution : Solver {

    CubeSet FromString(string input)
    {
        var rStr = Regex.Match(input, @"(\d+)\sred").Groups[1].Value;
        var gStr = Regex.Match(input, @"(\d+)\sgreen").Groups[1].Value;
        var bStr = Regex.Match(input, @"(\d+)\sblue").Groups[1].Value;

        var red = string.IsNullOrEmpty(rStr) ? 0 : int.Parse(rStr);
        var green = string.IsNullOrEmpty(gStr) ? 0 : int.Parse(gStr);
        var blue = string.IsNullOrEmpty(bStr) ? 0 : int.Parse(bStr);

        return new(red,green,blue);
    }
    record CubeSet(int Red, int Green, int Blue);
    record Game(int Id, IEnumerable<CubeSet> Sets, CubeSet MiniumSet);

    public object PartOne(string input) {
        var games = ReadGames(input);
        return SumIdsOfPossibleGames(games);
    }

    public object PartTwo(string input) {
        var games = ReadGames(input);
        var powerOfMinSets = PowerOfMiniumSets(games);
        return powerOfMinSets.Sum();
    }

    IEnumerable<Game> ReadGames(string input)
    {
        const string splitRegex = @"Game (\d+):([\d\sgren,dblu]+;?)+";
        var games = new List<Game>();
        var lines = input.Split('\n');
        foreach(var line in lines)
        {
            var splittedLine = Regex.Match(line, splitRegex);
            var id = int.Parse(splittedLine.Groups[1].Value);
            var cubeSets = new List<CubeSet>();
            foreach(var set in splittedLine.Groups[2].Captures.Select(i => i.Value))
            {
                cubeSets.Add(FromString(set));
            }
            games.Add(new(id, cubeSets, CalculateMinimumSet(cubeSets)));
        }
        return games;
    }

    bool GameIsPossible(Game game, int red, int green, int blue)
    {
        foreach(var set in game.Sets)
        {
            if(set.Red > red || set.Green > green || set.Blue > blue)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerable<Game> PossibleGames(IEnumerable<Game> games)
    {
            return games.Where(i => GameIsPossible(i, 12, 13, 14));
    }

    int SumIdsOfPossibleGames(IEnumerable<Game> games)
    {
       return PossibleGames(games).Sum(i => i.Id);
    }

    CubeSet CalculateMinimumSet(IEnumerable<CubeSet> sets)
    {
        var rMax = sets.Select(i => i.Red).Max();
        var gMax = sets.Select(i => i.Green).Max();
        var bMax = sets.Select(i => i.Blue).Max();
        return new(rMax, gMax, bMax);
    }

    IEnumerable<int> PowerOfMiniumSets(IEnumerable<Game> games)
    {
        return games.Select(i => i.MiniumSet.Red * i.MiniumSet.Green * i.MiniumSet.Blue);
    }
}
