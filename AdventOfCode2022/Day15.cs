using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode_2022
{
    internal class Day15 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(15);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // If shorter data length, run using part Part 1 logic (mostly brute force)
            if (lines.Length < 15) Part1Logic(lines);
            else RunFullData(lines);

            // Print performance summary
            Summary(watch);
        }

        private static void RunFullData(string[] lines)
        {
            // Initialize variables
            var beacons = new BeaconSet();

            // Parse data into beacon range slices
            Console.WriteLine("Converting input into range slices...");
            foreach(var line in lines)
            {
                beacons.Add(line);
            }

            // Find distress beacon
            var position = beacons.FindMissing();
            var frequency = (position.x * 4000000) + position.y;

            // Output results
            Console.WriteLine("The position of the missing distress beacon is " + position + ".");
            Console.WriteLine("The tuning frequency therefore must be " + frequency + ".");
        }

        private static void Part1Logic(string[] lines)
        {
            // Initialize variables
            var map = new SensorMap();
            var rowCheck = 10;

            // Parse input
            Console.WriteLine("Adding sensors and beacons to the grid...");
            foreach (var line in lines)
            {
                map.AddToGrid(line, rowCheck);
            }

            // Print map
            Console.WriteLine("\nPrinting map...");
            map.Print();

            // Output results and performance summary
            Console.WriteLine("\nIn row #" + rowCheck + ", there are " + map.CheckRow(rowCheck) + " positions which do not contain a beacon.");
        }
    }

    internal class BeaconSet : Map
    {
        internal Dictionary<long, List<(long min, long max)>> rows = new Dictionary<long, List<(long min, long max)>>();

        public BeaconSet()
        {
        }

        internal void Add(string line)
        {
            var coordinates = ParseLine(line);

            var range = Math.Abs(coordinates[0].x - coordinates[1].x)
                        + Math.Abs(coordinates[0].y - coordinates[1].y);

            for (long y = coordinates[0].y - range; y <= coordinates[0].y + range; y++)
            {
                long xRange = range - Math.Abs(coordinates[0].y - y);
                long min = Math.Max(coordinates[0].x - xRange, 0);
                long max = Math.Min(coordinates[0].x + xRange, 4000000);

                if (!rows.ContainsKey(y)) rows.Add(y, new List<(long x, long y)>());
                rows[y].Add((min, max));
            }
        }

        internal (long x, long y) FindMissing()
        {
            for (int j = 0; j <= 4000000; j++)
            {
                if (!rows.ContainsKey(j)) continue;

                long maxRange = 0;

                var row = rows[j];
                row.Sort();

                for (int i = 1; i < row.Count; i++)
                {
                    if (row[i - 1].max > maxRange) maxRange = row[i - 1].max;
                    if (maxRange > 4000000) continue;
                    if (row[i].min > maxRange) return (row[i].min - 1, j);
                }
            }

            return (-1, -1);
        }
    }

    internal class Map
    {
        internal (int x, int y)[] ParseLine(string line)
        {
            var findX = new Regex(@"(?<=(x=)).*?(?=,)");
            var findY = new Regex(@"(?<=(y=)).*");
            var segments = line.Split(": ");
            var result = new (int x, int y)[2];

            // Find 
            var sensorX = findX.Match(segments[0]);
            result[0].x = Convert.ToInt32
                        (segments[0]
                        .Substring(sensorX.Index, sensorX.Length));

            var sensorY = findY.Match(segments[0]);
            result[0].y = Convert.ToInt32
                        (segments[0]
                        .Substring(sensorY.Index, sensorY.Length));

            var beaconX = findX.Match(segments[1]);
            result[1].x = Convert.ToInt32
                        (segments[1]
                        .Substring(beaconX.Index, beaconX.Length));

            var beaconY = findY.Match(segments[1]);
            result[1].y = Convert.ToInt32
                        (segments[1]
                        .Substring(beaconY.Index, beaconY.Length));

            return result;
        }
    }

    internal class SensorMap : Map
    {
        internal Dictionary<(int x, int y), char> Grid = new Dictionary<(int x, int y), char>();
        internal int maxX = Int32.MinValue;
        internal int minX = Int32.MaxValue;
        internal int maxY = Int32.MinValue;
        internal int minY = Int32.MaxValue;

        public SensorMap()
        {
        }

        internal void AddToGrid(string line, int rowCheck)
        {
            var coordinates = ParseLine(line);

            foreach(var location in coordinates)
            {
                CheckBounds(location.x, location.y);
            }

            if (Grid.ContainsKey(coordinates[0])) Grid[coordinates[0]] = 'S';
            else Grid.Add(coordinates[0], 'S');

            if (Grid.ContainsKey(coordinates[1])) Grid[coordinates[1]] = 'B';
            else Grid.Add(coordinates[1], 'B');

            CalculateSensorRange(coordinates[0], coordinates[1], rowCheck);
        }

        private void CalculateSensorRange((int x, int y) sensor, (int x, int y) beacon, int rowCheck)
        {
            var range = Math.Abs(sensor.x - beacon.x)
                        + Math.Abs(sensor.y - beacon.y);

            for (int y = sensor.y - range; y <= sensor.y + range; y++)
            {
                if (y == rowCheck || rowCheck == 10)
                {
                    var xRange = range - Math.Abs(sensor.y - y);
                    for (int x = sensor.x - xRange; x <= sensor.x + xRange; x++)
                    {
                        Grid.TryAdd((x, y), '#');
                        CheckBounds(x, y);
                    }
                }
            }
        }

        // Checks a set of x,y coordinates agains the current max and min values
        private void CheckBounds(int x, int y)
        {
            if (x > maxX) maxX = x;
            if (x < minX) minX = x;
            if (y > maxY) maxY = y;
            if (y < minY) minY = y;
        }

        internal void Print()
        {
            for (int y = minY; y <= maxY; y++)
            {
                Console.Write(y
                    .ToString()
                    .PadLeft(maxY
                    .ToString()
                    .Length) + ' ');

                for (int x = minX; x <= maxX; x++)
                {
                    if (Grid.ContainsKey((x, y))) Console.Write(Grid[(x, y)]);
                    else Console.Write('.');
                }
                Console.WriteLine();
            }
        }

        internal int CheckRow(int y)
        {
            var nonBeacon = 0;

            for (int x = minX; x <= maxX; x++)
            {
                if (Grid.ContainsKey((x, y)))
                {
                    if (Grid[(x, y)] != 'B') nonBeacon++;
                }
            }

            return nonBeacon;
        }
    }
}