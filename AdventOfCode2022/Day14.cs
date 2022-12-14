using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode_2022
{
    internal class Day14 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(14);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Variable initialization
            var scan = new MapScan();
            var sandCount = 0;
            var full = false;

            // Parse data
            foreach(var line in lines)
            {
                scan.AddToMap(line);
            }

            // Process sand falling
            while(!full)
            {
                var moving = true;
                (int x, int y) currentPos = (500, 0);

                // Loop until our unit of sand comes to rest
                while(moving)
                {

                    if (scan.SpaceEmpty(currentPos.x, currentPos.y + 1))
                    {
                        currentPos.y++;
                    }
                    else if (scan.SpaceEmpty(currentPos.x - 1, currentPos.y + 1))
                    {
                        currentPos.x--;
                        currentPos.y++;
                    }
                    else if (scan.SpaceEmpty(currentPos.x + 1, currentPos.y + 1))
                    {
                        currentPos.x++;
                        currentPos.y++;
                    }
                    else if (currentPos == (500, 0))
                    {
                        moving = false;
                        full = true;
                        sandCount++;
                    }
                    else
                    {
                        scan.AddSand(currentPos.x, currentPos.y);
                        moving = false;
                        sandCount++;
                    }
                }
            }

            // Output results and performance summary
            Console.WriteLine(sandCount + " units of sand will fill the area.");
            Summary(watch);
        }
    }

    internal class MapScan
    {
        internal Dictionary<(int x, int y), char> Grid = new Dictionary<(int x, int y), char>();
        internal int LowestRock = 0;

        public MapScan()
        {
            Grid.Add((500, 0), '+');
        }

        internal void AddSand(int x, int y)
        {
            Grid.Add((x, y), 'O');
        }

        internal void AddToMap(string line)
        {
            var coordinates = line.Split(" -> ");

            for (int i = 1; i < coordinates.Length; i++)
            {
                AddLine(coordinates[i - 1], coordinates[i]);
            }
        }

        internal bool SpaceEmpty(int x, int y)
        {
            if (y >= LowestRock + 2) return false;

            return !Grid.ContainsKey((x, y));
        }

        private void AddLine(string point1, string point2)
        {
            var XY1 = point1.Split(',');
            var XY2 = point2.Split(',');

            var x1 = Convert.ToInt32(XY1[0]);
            var y1 = Convert.ToInt32(XY1[1]);
            var x2 = Convert.ToInt32(XY2[0]);
            var y2 = Convert.ToInt32(XY2[1]);

            var xMax = Math.Max(x1, x2);
            var xMin = Math.Min(x1, x2);
            var yMax = Math.Max(y1, y2);
            var yMin = Math.Min(y1, y2);

            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    Grid.TryAdd((x, y), '#');
                    if (y > LowestRock) LowestRock = y;
                }
            }
        }
    }
}