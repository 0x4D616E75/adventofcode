using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.NetworkInformation;
using AngleSharp.Common;

namespace AdventOfCode.Y2023.Day18;

[ProblemName("Lavaduct Lagoon")]
class Solution : Solver {

    public object PartOne(string input) {
        var digs = Parse1(input);
        var points = GeneratePoints(digs);
        var i = CalculateAreaOfPoligon(points);
        var r = digs.Select(i => i.Meters).Sum();
        return CalculatePixsTheorem(r, i);
    }

    public object PartTwo(string input) {
        var digs = Parse2(input);
        var points = GeneratePoints(digs);
        var i = CalculateAreaOfPoligon(points);
        var r = digs.Select(i => i.Meters).Sum();
        return CalculatePixsTheorem(r, i);
    }

    record Dig(string Direction, int Meters, string Color);

    IEnumerable<Dig> Parse1(string input)
    {
        var matches = Regex.Matches(input, @"(U|D|L|R) (\d+) \(#([\da-f]+)\)");
        return matches.Select(i => new Dig(i.Groups[1].Value, int.Parse(i.Groups[2].Value), i.Groups[3].Value)).ToHashSet();
    }

    IEnumerable<Dig> Parse2(string input)
    {
        var matches = Regex.Matches(input, @"\(#([\da-f]{5})([\d])\)");
        return matches.Select(i => new Dig(ConvertPart2(i.Groups[2].Value), Convert.ToInt32(i.Groups[1].Value, 16), null)).ToHashSet();
    }

    string ConvertPart2(string input) => input switch {
        "0" => "R",
        "1" => "D",
        "2" => "L",
        "3" => "U",
        _ => throw new ArgumentOutOfRangeException()
    };

    record Point(double X, double Y);
    IEnumerable<Point> GeneratePoints(IEnumerable<Dig> digs)
    {
        var points = new HashSet<Point>();
        double x = 0.0, y = 0.0;
        points.Add(new Point(x, y));
        foreach(var dig in digs)
        {
            switch(dig.Direction)
            {
                case "U":
                    y -= dig.Meters;
                    break;
                case "D":
                    y += dig.Meters;
                    break;
                case "L":
                    x -= dig.Meters;
                    break;
                case "R":
                    x += dig.Meters;
                    break;
            }
            points.Add(new Point(x, y));
        }
        return points;
    }

    double CalculateAreaOfPoligon(IEnumerable<Point> points)
    {
        var pointCount = points.Count();
        double area = 0.0;
        for(var i = 0; i < pointCount; i++)
        {
            var p1 = points.ElementAt(i);
            var p2 = i + 1 < pointCount ? points.ElementAt(i + 1) : points.ElementAt(0);
            area += (p2.X - p1.X) * (p2.Y + p1.Y);
        }

        return Math.Abs(area / 2.0);
    }

    double CalculatePixsTheorem(double r, double i)
    {
        var a = i + r/2 + 1;
        return a;
    }
}
