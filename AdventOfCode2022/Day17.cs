using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode_2022
{
    internal class Day17 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(17);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Initialize variables
            var jetsBlown = 0;
            var newRock = true;
            Rock currentRock = new Rock("square", 4);
            var rockCount = 0;
            string[] rockTypes = { "horizontal", "cross", "L", "vertical", "square" };
            var mainMap = new RockMap();

            // Parse data
            while(rockCount <= 2022)
            {
                if (jetsBlown == lines[0].Length) ;

                jetsBlown = jetsBlown % lines[0].Length;
                var direction = lines[0][jetsBlown];

                if (newRock)
                {
                    var type = rockTypes[rockCount % 5];

                    currentRock = new Rock(type, mainMap.Top + 4);

                    newRock = false;
                    rockCount++;
                }

                if (direction == '>')
                {
                    currentRock.X++;
                    if (currentRock.CollisionCheck(mainMap)) currentRock.X--;
                }
                else
                {
                    currentRock.X--;
                    if (currentRock.CollisionCheck(mainMap)) currentRock.X++;
                }

                currentRock.Y--;
                if (currentRock.CollisionCheck(mainMap))
                {
                    currentRock.Y++;
                    mainMap.AddRock(currentRock);
                    //Console.Clear();
                    //Console.WriteLine("Rock #" + rockCount);
                    //mainMap.PrintMap();
                    newRock = true;
                }

                jetsBlown++;
            }

            // Output results and performance summary
            mainMap.PrintMap();
            Console.WriteLine("The height of the stack of rocks after 2022 rocks is " + mainMap.Top + " unit.");
            Summary(watch);

            // 3421 is too high
        }
    }

    internal class RockMap
    {
        internal int Top = 0;
        internal Dictionary<(int x, int y), char> Map = new Dictionary<(int x, int y), char>();

        public RockMap()
        {
        }

        internal void AddRock(Rock currentRock)
        {
            switch (currentRock.Type)
            {
                case "horizontal":
                    for (int i = 0; i < 4; i++)
                    {
                        Map.Add((currentRock.X + i, currentRock.Y), '@');
                        Top = Math.Max(Top, currentRock.Y);
                    }
                    break;
                case "cross":
                    Map.Add((currentRock.X + 1, currentRock.Y + 2), '@');
                    Map.Add((currentRock.X + 1, currentRock.Y), '@');
                    for (int i = 0; i < 3; i++)
                    {
                        Map.Add((currentRock.X + i, currentRock.Y + 1), '@');
                    }
                    Top = Math.Max(Top, currentRock.Y + 2);
                    break;
                case "L":
                    for (int i = 0; i < 3; i++)
                    {
                        Map.Add((currentRock.X + i, currentRock.Y), '@');
                    }
                    for (int i = 1; i < 3; i++)
                    {
                        Map.Add((currentRock.X + 2, currentRock.Y + i), '@');
                    }

                    Top = Math.Max(Top, currentRock.Y + 2);
                    break;
                case "vertical":
                    for (int i = 0; i < 4; i++)
                    {
                        Map.Add((currentRock.X, currentRock.Y + i), '@');
                    }

                    Top = Math.Max(Top, currentRock.Y + 3);
                    break;
                case "square":
                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < 2; j++)
                            Map.Add((currentRock.X + i, currentRock.Y + j), '@');
                    Top = Math.Max(Top, currentRock.Y + 1);
                    break;
            }
        }

        internal void PrintMap()
        {
            for (int y = Top; y >= Top - 10 && y > 0; y--)
            {
                Console.Write(y.ToString().PadLeft(4));
                for (int x = 0; x <= 8; x++)
                {
                    switch (x)
                    {
                        case 0:
                            Console.Write('|');
                            break;
                        case 8:
                            Console.WriteLine('|');
                            break;
                        default:
                            if (Map.ContainsKey((x, y))) Console.Write(Map[(x, y)]);
                            else Console.Write('.');
                            break;
                    }
                }
            }
            Console.WriteLine("+-------+");
        }
    }

    internal class Rock
    {
        internal int X = 3;
        internal int Y;
        internal string Type;

        public Rock(string type, int startingHeight)
        {
            Y = startingHeight;
            Type = type;
        }

        public bool CollisionCheck(RockMap map)
        {
            if (Y == 0) return true;

            switch (Type)
            {
                case "horizontal":
                    if (X < 1 || X + 3 > 7) return true;
                    for (int i = 0; i < 4; i++)
                    {
                        if (map.Map.ContainsKey((X + i, Y))) return true;
                    }
                    break;
                case "cross":
                    if (X < 1 || X + 2 > 7) return true;
                    if (map.Map.ContainsKey((X + 1, Y + 2))) return true;
                    if (map.Map.ContainsKey((X + 1, Y))) return true;
                    for (int i = 0; i < 3; i++)
                    {
                        if (map.Map.ContainsKey((X + i, Y + 1))) return true;
                    }
                    break;
                case "L":
                    if (X < 1 || X + 3 > 7) return true;
                    for (int i = 0; i < 3; i++)
                    {
                        if (map.Map.ContainsKey((X + i, Y))) return true;
                    }
                    for (int i = 1; i < 3; i++)
                    {
                        if (map.Map.ContainsKey((X + 2, Y + i))) return true;
                    }
                    break;
                case "vertical":
                    if (X < 1 || X > 7) return true;
                    for (int i = 0; i < 4; i++)
                    {
                        if (map.Map.ContainsKey((X, Y + i))) return true;
                    }
                    break;
                case "square":
                    if (X < 1 || X + 1 > 7) return true;
                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < 2; j++)
                            if (map.Map.ContainsKey((X + i, Y + j))) return true;
                    break;
            }

            return false;
        }
    }
}