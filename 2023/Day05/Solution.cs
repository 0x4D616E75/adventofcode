using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Common;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]
class Solution : Solver {

    public object PartOne(string input) {
        var almanac = Parse(input);
        var location = new List<long>();
        foreach(var seed in almanac.Seeds)
        {
            location.Add(almanac.GetLocationOfSeed(seed));
        }
        return location.Min();
    }

    public object PartTwo(string input) {
        var almanac = Parse(input);
        long minLocation = -1;
        foreach(var seed in almanac.SeedsWithRange)
        {
            for(long i = 0; i < seed.Length; i++)
            {
                var location = almanac.GetLocationOfSeed(seed.Start + i);
                if(minLocation == -1)
                {
                    minLocation = location;
                    continue;
                }
                if(minLocation > location)
                {
                    minLocation = location;
                }
            }
        }
        return minLocation;
    }


    Almanac Parse(string input)
    {
        var parts = input.Split("\n\n");
        var seeds = Regex.Matches(parts[0], @"\d+").Select(i => long.Parse(i.Value));
        var seedToSoil = Regex.Matches(parts[1], @"\d+").Select(i => long.Parse(i.Value));
        var soilToFertilizer = Regex.Matches(parts[2], @"\d+").Select(i => long.Parse(i.Value));
        var fertilizerToWater = Regex.Matches(parts[3], @"\d+").Select(i => long.Parse(i.Value));
        var waterToLight = Regex.Matches(parts[4], @"\d+").Select(i => long.Parse(i.Value));
        var lightToTemperature = Regex.Matches(parts[5], @"\d+").Select(i => long.Parse(i.Value));
        var temperatureToHumidity = Regex.Matches(parts[6], @"\d+").Select(i => long.Parse(i.Value));
        var humidityToLocation = Regex.Matches(parts[7], @"\d+").Select(i => long.Parse(i.Value));
        return new Almanac(seeds, seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation);
    }

    record Entry
    {
        public long Destination;
        public long Source;
        public long Length;
        public long SourceEnd;
        public Entry(long destination, long source, long length)
        {
            Destination = destination;
            Source = source;
            Length = length;
            SourceEnd = source + length;
        }
    }

    record SeedRange(long Start, long Length);
    record Almanac
    {
        public IEnumerable<long> Seeds;
        public IEnumerable<SeedRange> SeedsWithRange;
        IEnumerable<Entry> SeedToSoil;
        IEnumerable<Entry> SoilToFertilizer;
        IEnumerable<Entry> FertilizerToWater;
        IEnumerable<Entry> WaterToLight;
        IEnumerable<Entry> LightToTemperature;
        IEnumerable<Entry> TemperatureToHumidity;
        IEnumerable<Entry> HumidityToLocation;
        public Almanac(IEnumerable<long> seeds,
                   IEnumerable<long> seedToSoil,
                   IEnumerable<long> soilToFertilizer,
                   IEnumerable<long> fertilizerToWater,
                   IEnumerable<long> waterToLight,
                   IEnumerable<long> lightToTemperature,
                   IEnumerable<long> temperatureToHumidity,
                   IEnumerable<long> humidityToLocation)
        {
            Seeds = seeds;
            SeedsWithRange = ExtractSeedsWithRange(seeds);
            SeedToSoil = GenerateMap(seedToSoil);
            SoilToFertilizer = GenerateMap(soilToFertilizer);
            FertilizerToWater = GenerateMap(fertilizerToWater);
            WaterToLight = GenerateMap(waterToLight);
            LightToTemperature = GenerateMap(lightToTemperature);
            TemperatureToHumidity = GenerateMap(temperatureToHumidity);
            HumidityToLocation = GenerateMap(humidityToLocation);
        }



        public long GetLocationOfSeed(long seed)
        {
            var soil = GetValue(SeedToSoil, seed);
            var fertilizer = GetValue(SoilToFertilizer, soil);
            var water = GetValue(FertilizerToWater, fertilizer);
            var light = GetValue(WaterToLight, water);
            var temperature = GetValue(LightToTemperature, light);
            var humidity = GetValue(TemperatureToHumidity, temperature);
            var location = GetValue(HumidityToLocation, humidity);
            return location;
        }
        long GetValue(IEnumerable<Entry> map, long source)
        {
            var value = map.Where(i => source >= i.Source && source <= i.SourceEnd).SingleOrDefault();
            if(value != null)
            {
                return value.Destination + (source - value.Source);
            }
            else
            {
                return source;
            }
        }
        IEnumerable<Entry> GenerateMap(IEnumerable<long> inMap)
        {
            var outMap = new List<Entry>();
            for(var i = 0; i < inMap.Count(); i += 3)
            {
                var dest = inMap.GetItemByIndex(i);
                var src = inMap.GetItemByIndex(i+1);
                var len = inMap.GetItemByIndex(i+2);
                outMap.Add(new(dest, src, len));
            }
            return outMap;
        }
        IEnumerable<SeedRange> ExtractSeedsWithRange(IEnumerable<long> seeds)
        {
            var seedsWithRange = new List<SeedRange>();
            for(var i = 0; i < seeds.Count(); i += 2)
            {
                var start = seeds.GetItemByIndex(i);
                var len = seeds.GetItemByIndex(i+1);
                seedsWithRange.Add(new SeedRange(start, len));
            }
            return seedsWithRange;
        }
    }
}
