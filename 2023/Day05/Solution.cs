using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Common;
using System.Numerics;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;

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
        long minLocation = long.MaxValue;
        // Parallel.ForEach(almanac.SeedsWithRange, seed => {
        //     for(var i = 0; i < seed.Length; i++)
        //     {
        //         var t = almanac.GetSeedFromLocation(i);
        //         if(seed.Start >= t && t < seed.End)
        //         {
        //             if(t < minLocation)
        //             {
        //                 minLocation = t;
        //             }
        //             break;
        //         }
        //     }
        // });

        foreach(var seed in almanac.SeedsWithRange)
        {
            Parallel.For(0, seed.Length, i => 
            {
                var location = almanac.GetLocationOfSeed(seed.Start + i);
                if(minLocation > location)
                {
                    minLocation = location;
                }
            });
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

    record Map
    {
        public Range Destination;
        public Range Source;
        public Map(long destination, long source, long length)
        {
            Destination = new (destination, length);
            Source = new (source, length);
        }
    }

    record Range
    {
        public long Start;
        public long End;
        public long Length;
        public Range(long start, long length)
        {
            Start = start;
            End = start + length;
            Length = length;
        }
    }
    record Almanac
    {
        public IEnumerable<long> Seeds;
        public IEnumerable<Range> SeedsWithRange;
        IEnumerable<Map> SeedToSoil;
        IEnumerable<Map> SoilToFertilizer;
        IEnumerable<Map> FertilizerToWater;
        IEnumerable<Map> WaterToLight;
        IEnumerable<Map> LightToTemperature;
        IEnumerable<Map> TemperatureToHumidity;
        IEnumerable<Map> HumidityToLocation;
        IEnumerable<Map> BSeedToSoil;
        IEnumerable<Map> BSoilToFertilizer;
        IEnumerable<Map> BFertilizerToWater;
        IEnumerable<Map> BWaterToLight;
        IEnumerable<Map> BLightToTemperature;
        IEnumerable<Map> BTemperatureToHumidity;
        IEnumerable<Map> BHumidityToLocation;
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

            BSeedToSoil = BGenerateMap(seedToSoil);
            BSoilToFertilizer = BGenerateMap(soilToFertilizer);
            BFertilizerToWater = BGenerateMap(fertilizerToWater);
            BWaterToLight = BGenerateMap(waterToLight);
            BLightToTemperature = BGenerateMap(lightToTemperature);
            BTemperatureToHumidity = BGenerateMap(temperatureToHumidity);
            BHumidityToLocation = BGenerateMap(humidityToLocation);
        }

        public bool IsInRange(long seed)
        {
            return SeedsWithRange.Count(i => i.Start >= seed && seed < i.End) > 0;
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

        public long GetSeedFromLocation(long location)
        {
            var humidity = GetValue(BHumidityToLocation, location);
            var temperature = GetValue(BTemperatureToHumidity, humidity);
            var light = GetValue(BLightToTemperature, temperature);
            var water = GetValue(BWaterToLight, light);
            var fertilizer = GetValue(BFertilizerToWater, water);
            var soil = GetValue(BSoilToFertilizer, fertilizer);
            var seed = GetValue(BSeedToSoil, soil);
            
            return seed;
        }
        long GetValue(IEnumerable<Map> map, long source)
        {
            foreach(var entry in map)
            {
                if(source >= entry.Source.Start && source < entry.Source.End)
                {
                    return entry.Destination.Start + (source - entry.Source.Start);
                }
            }

            return source;
        }
        IEnumerable<Map> GenerateMap(IEnumerable<long> inMap)
        {
            return inMap.Chunk(3).Select(i => new Map(i[0],i[1],i[2]));
        }
        IEnumerable<Map> BGenerateMap(IEnumerable<long> inMap)
        {
            var t= inMap.Chunk(3).Select(i => new Map(i[1],i[0],i[2]));
            return t;
        }
        IEnumerable<Range> ExtractSeedsWithRange(IEnumerable<long> seeds)
        {
            return seeds.Chunk(2).Select(i => new Range(i[0], i[1]));
        }
    }
}
