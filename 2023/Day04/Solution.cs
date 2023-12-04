using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Common;
using System.Drawing;

namespace AdventOfCode.Y2023.Day04;

[ProblemName("Scratchcards")]
class Solution : Solver {

    public object PartOne(string input) {
        var cards = ParseCards(input);
        return cards.Select(i => i.Points).Sum();
    }

    public object PartTwo(string input) {
        var cards = ParseCards(input);
        var copies = cards.SelectMany(i => i.GetCopies(cards));
        return cards.Count() + copies.Count();
    }

    record Card
    {
        public int Number { get; }
        public IEnumerable<int> WinningNumbers { get; }
        public IEnumerable<int> MyNumbers { get;}
        public int MatchCount { get; }
        public int Points { get; }

        public Card(int number, IEnumerable<int> winningNumbers, IEnumerable<int> myNumbers)
        {
            Number = number;
            WinningNumbers = winningNumbers;
            MyNumbers = myNumbers;
            MatchCount = MyNumbers.Intersect(WinningNumbers).Count();
            Points = (int)(MatchCount == 0 ? 0 : Math.Pow(2, MatchCount-1));
        }

        public IEnumerable<Card> GetCopies(IEnumerable<Card> cards)
        {
            var copies = new List<Card>();
            for(var i = 0; i < MatchCount; i++)
            {
                var idx = Number + i;
                if(idx < cards.Count())
                {
                    var copy = cards.ElementAt(idx);
                    copies.Add(copy);
                    copies.AddRange(copy.GetCopies(cards));
                }
            }
            return copies;
        }
    }

    IEnumerable<Card> ParseCards(string input)
    {
        var cards = new List<Card>();
        var lines = input.Split('\n');
        foreach(var line in lines)
        {
            var card = Regex.Match(line, @"Card\s+(\d+):(.+)\|(.+)");
            var number = int.Parse(card.Groups[1].Value);
            var winningNumbers = Regex.Matches(card.Groups[2].Value, @"\d+").Select(i => int.Parse(i.Value));
            var myNumbers = Regex.Matches(card.Groups[3].Value, @"\d+").Select(i => int.Parse(i.Value));
            cards.Add(new (number, winningNumbers, myNumbers));
        }
        return cards;   
    }
   
}
