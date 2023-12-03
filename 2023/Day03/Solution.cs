using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day03;

[ProblemName("Gear Ratios")]
class Solution : Solver {

    public object PartOne(string input) {
        var possibleEngineParts = ParsePossibleEngineParts(input);
        var symbols = ParseSymbols(input, @"(?!\d+|\.).");
        var engineParts = ExtractEngineParts(possibleEngineParts, symbols);
        return engineParts.Sum();
    }

    public object PartTwo(string input) {
        var engineParts = ParsePossibleEngineParts(input);
        var possibleGears = ParseSymbols(input, @"\*");
        var gears = ExtractGears(possibleGears,engineParts);
        return gears.Select(gear => gear.Ratio).Sum();
    }


    record Symbol(int Col, int Row);
    record PossibleEnginePart(string Value, int Col, int Row);
    record _Gear(int FirstEnginePart, int SecondEnginePart)
    {
        public int Ratio => FirstEnginePart * SecondEnginePart;
    }

    IEnumerable<Symbol> ParseSymbols(string input, string symbolRegex)
    {
        var symbol = new List<Symbol>();
        var lines = input.Split('\n');
        for(var i = 0; i< lines.Count(); i++)
        {
            var matches = Regex.Matches(lines[i], symbolRegex);
            symbol.AddRange(matches.Select(match => new Symbol(match.Index, i)));
        }
        return symbol;
    }

    IEnumerable<PossibleEnginePart> ParsePossibleEngineParts(string input)
    {
        var parts = new List<PossibleEnginePart>();
        var lines = input.Split('\n');
        for(var i = 0; i< lines.Count(); i++)
        {
            var matches = Regex.Matches(lines[i], @"\d+");
            parts.AddRange(matches.Select(match => new PossibleEnginePart(match.Value, match.Index, i)));
        }
        return parts;
    }

    bool IsSymbolNear(Symbol symbol, PossibleEnginePart part)
    {
        // Manhattan distance between each point of enginePart and gear
        var rowDistance = Math.Abs(symbol.Row - part.Row);
        if(rowDistance > 1)
        {
            return false;
        }
        for(var i = 0; i < part.Value.Length; i++)
        {
            if(Math.Abs(symbol.Col - (part.Col + i)) <= 1)
            {
                return true;
            }
        }
        return false;
    }
    bool IsEnginePart(PossibleEnginePart possibleEnginePart, IEnumerable<Symbol> symbols)
    {
        return symbols.Where(i => IsSymbolNear(i, possibleEnginePart)).Count() > 0;
    }
    IEnumerable<int> ExtractEngineParts(IEnumerable<PossibleEnginePart> possibleEngineParts, IEnumerable<Symbol> symbols)
    {
        return possibleEngineParts.Where(i => IsEnginePart(i, symbols)).Select(i => int.Parse(i.Value));
    }

    IEnumerable<_Gear> ExtractGears(IEnumerable<Symbol> possibleGears, IEnumerable<PossibleEnginePart> possibleEngineParts)
    {
        var gears = new List<_Gear>();
        foreach(var gear in possibleGears)
        {
            int? firstPart = null, secondPart = null;
            foreach(var part in possibleEngineParts)
            {
                if(IsSymbolNear(gear, part))
                {
                    if(firstPart == null)
                    {
                        firstPart = int.Parse(part.Value);
                        continue;
                    }
                    
                    if(secondPart == null)
                    {
                        secondPart = int.Parse(part.Value);
                        continue;
                    }
                    
                    // Gear has more than two parts
                    firstPart = null; 
                    secondPart = null;
                    break;
                }
            }
            
            if(firstPart != null && secondPart != null)
            {
                gears.Add(new _Gear((int)firstPart, (int)secondPart));
            }
        }
        return gears;
    }
}
