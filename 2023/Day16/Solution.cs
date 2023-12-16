using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Common;

namespace AdventOfCode.Y2023.Day16;

[ProblemName("The Floor Will Be Lava")]
class Solution : Solver {

    public object PartOne(string input) {
        var map = Parse(input);
        var usedTiles = GetUsedTiles(map, 0, 0, Direction.Right);
        return usedTiles.Count();
    }

    public object PartTwo(string input) {
        var map = Parse(input);
        var inputs = GetAllInputs(map);
        var usedTiles = inputs.Select(i => GetUsedTiles(map, i.Item1, i.Item2, i.Item3));
        return usedTiles.Select(i => i.Count()).Max();
    }

    char[][] Parse(string input)
    {
        var lines = input.Split('\n');
        var map = new char[lines.Count()][];
        for(var i = 0; i < map.Length; i++)
        {
            map[i] = lines[i].ToCharArray();
        }
        return map;
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
    record Tile(int X, int Y, Direction Dir, char Value);

    class TileComparer : IEqualityComparer<Tile>
    {
        public bool Equals(Tile x, Tile y) => x.X == y.X && x.Y == y.Y;

        public int GetHashCode([DisallowNull] Tile obj) => obj.Value.GetHashCode();
    }

    Tile GetNext(char[][] map, int x, int y, Direction direction)
    {
        if(x < 0 || x >= map[0].Length || y < 0 || y >= map.Length)
        {
            return null;
        }
        return new(x, y, direction, map[y][x]);
    }

    Tile[] NextSteps(char[][] map, Tile start, IEnumerable<Tile> knownSteps)
    {
        if(knownSteps.Contains(start))
        {
            return [null, null];
        }

        Tile[] nexts = start switch
        {
            { Value: '|', Dir: var dir } when dir == Direction.Right || dir == Direction.Left => [GetNext(map, start.X, start.Y - 1, Direction.Up), GetNext(map, start.X, start.Y + 1, Direction.Down)],
            { Value: '-', Dir: var dir } when dir == Direction.Up || dir == Direction.Down => [GetNext(map, start.X - 1, start.Y, Direction.Left), GetNext(map, start.X + 1, start.Y, Direction.Right)],
            { Value: '/', Dir: Direction.Up } => [GetNext(map, start.X + 1, start.Y , Direction.Right ), null], 
            { Value: '/', Dir: Direction.Right } => [GetNext(map, start.X, start.Y - 1 , Direction.Up ), null], 
            { Value: '/', Dir: Direction.Down } => [GetNext(map, start.X - 1, start.Y , Direction.Left ), null], 
            { Value: '/', Dir: Direction.Left } => [GetNext(map, start.X, start.Y + 1 , Direction.Down ), null], 
            { Value: '\\', Dir: Direction.Up } => [GetNext(map, start.X - 1, start.Y , Direction.Left ), null], 
            { Value: '\\', Dir: Direction.Right } => [GetNext(map, start.X, start.Y + 1, Direction.Down ), null], 
            { Value: '\\', Dir: Direction.Down } => [GetNext(map, start.X + 1, start.Y , Direction.Right ), null], 
            { Value: '\\', Dir: Direction.Left } => [GetNext(map, start.X, start.Y - 1, Direction.Up ), null], 
            { Dir: Direction.Up } => [GetNext(map, start.X, start.Y - 1, Direction.Up ), null],
            { Dir: Direction.Right } => [GetNext(map, start.X + 1, start.Y, Direction.Right ), null],
            { Dir: Direction.Down } => [GetNext(map, start.X, start.Y + 1, Direction.Down ), null],
            { Dir: Direction.Left } => [GetNext(map, start.X - 1, start.Y, Direction.Left ), null],
            _ => throw new ArgumentOutOfRangeException()
        };

        return nexts;
    }

    Tile[] GetUsedTiles(char[][] map, int x, int y, Direction dir)
    {
        var usedTiles = new List<Tile>();
        var queue = new Queue<Tile>();
        queue.Enqueue(new Tile(x, y, dir, map[y][x]));
        while(queue.TryDequeue(out Tile next))
        {
            // Console.WriteLine($"X:{next.X} | Y:{next.Y} | Value:{next.Value} | Dir:{next.Dir}");

            var nexts = NextSteps(map, next, usedTiles);

            for(var i = 0; i < nexts.Length; i++)
            {
                if(nexts[i] != null)
                {
                    queue.Enqueue(nexts[i]);
                }
            }

            usedTiles.Add(next);
        }

        return usedTiles.Distinct(new TileComparer()).ToArray();
    }

    IEnumerable<(int,int, Direction)> GetAllInputs(char[][] map)
    {
        var yLength = map.Length;
        var xLength = map[0].Length;
        var inputs = new List<(int,int, Direction)>();
        inputs.AddRange(Enumerable.Range(0, yLength).Select(i => (0,i,Direction.Right)));
        inputs.AddRange(Enumerable.Range(0, yLength).Select(i => (xLength - 1,i,Direction.Left)));
        inputs.AddRange(Enumerable.Range(0, xLength).Select(i => (i,0,Direction.Down)));
        inputs.AddRange(Enumerable.Range(0, xLength).Select(i => (i,yLength - 1,Direction.Up)));
        return inputs;
    }
}
