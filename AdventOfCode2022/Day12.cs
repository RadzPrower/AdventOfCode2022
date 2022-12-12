using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode_2022
{
    // Some global variables for use between all methods without having to manually pass them each time
    public static class GlobalVar
    {
        public static (int x, int y) start = (0, 0);
        public static (int x, int y) end = (0, 0);
        public static Dictionary<(int x, int y), int> visitedNodes = new Dictionary<(int x, int y), int>();
        public static string[] lines;
        public static (int x, int y) scenic;
    }

    internal class Day12 : AoC2022
    {
        internal static void Start()
        {
            // Clear the visitedNodes variable in case this test is executed multiple times
            GlobalVar.visitedNodes.Clear();
            GlobalVar.scenic = (-1, -1);

            // Prompt the user to ask if this is a test
            GlobalVar.lines = TestPrompt(12);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Find start and end coordinates
            Console.WriteLine("Finding start and end points...\n");
            for (int y = 0; y < GlobalVar.lines.Length; y++)
            {
                for (int x = 0; x < GlobalVar.lines[y].Length; x++)
                {
                    switch (GlobalVar.lines[y][x])
                    {
                        case 'S': // Found starting point, save to global variable
                            GlobalVar.start = (x, y);
                            Console.WriteLine("Start point is " + GlobalVar.start);
                            break;
                        case 'E': // Found ending point, save to global variable
                            GlobalVar.end = (x, y);
                            Console.WriteLine("End point is " + GlobalVar.end);
                            break;
                    }
                }
            }

            // Find routes from end point back to starting point
            Console.WriteLine("\nPathfinding...\n");
            GlobalVar.visitedNodes.Add(GlobalVar.end, 0); // Add end node
            FindPath();

            // Output results and performance summary
            Console.WriteLine("The fewest steps required to get best signal is " + GlobalVar.visitedNodes[GlobalVar.start] + ".");
            Console.WriteLine("The length of the scenic hiking route is " + GlobalVar.visitedNodes[GlobalVar.scenic] + ".");
            Summary(watch);
        }

        // Find the path from the global end and start points
        internal static void FindPath()
        {
            // Loop until the start node is found and added to the Dictionary
            while (!GlobalVar.visitedNodes.ContainsKey(GlobalVar.start))
            {
                // Check ONLY nodes in the Dictionary at the start of the while loop
                for (int i = GlobalVar.visitedNodes.Count - 1; i >= 0; i--)
                {
                    // Loop variable initialization
                    var node = GlobalVar.visitedNodes.ElementAt(i);
                    var x = node.Key.x;
                    var y = node.Key.y;
                    var letter = GlobalVar.lines[y][x];
                    var height = Convert.ToInt32(letter);

                    // Check if the height is "zero" to start the scenic hiking trail
                    if (height == 97 && GlobalVar.scenic is (-1, -1))
                    {
                        GlobalVar.scenic = (x, y);
                        Console.WriteLine("Scenic route starting point is " + GlobalVar.scenic + "\n");
                    }

                    // Check each cardinal direction to see if it is possible to move there
                    if (ValidMove(x + 1, y, height)) GlobalVar.visitedNodes.TryAdd((x + 1, y), node.Value + 1);
                    if (ValidMove(x - 1, y, height)) GlobalVar.visitedNodes.TryAdd((x - 1, y), node.Value + 1);
                    if (ValidMove(x, y + 1, height)) GlobalVar.visitedNodes.TryAdd((x, y + 1), node.Value + 1);
                    if (ValidMove(x, y - 1, height)) GlobalVar.visitedNodes.TryAdd((x, y - 1), node.Value + 1);
                }
            }
        }

        // Check if the input coordinates would be a valid move
        private static Boolean ValidMove(int x, int y, int height)
        {
            // Ensure that x and y are within the bounds of the map
            if (x < 0 || y < 0) return false;
            if (y >= GlobalVar.lines.Length || x >= GlobalVar.lines[y].Length) return false;

            // Check if the coordinates have already been visited
            if (!GlobalVar.visitedNodes.ContainsKey((x, y)))
            {
                // Set value of 'E' point
                if (height == 69) height = 122;

                // Pull coordinates height from map data
                var tempHeight = Convert.ToInt32(GlobalVar.lines[y][x]);

                // Set value of 'S' point
                if (tempHeight == 83) tempHeight = 97;

                // Check if the height difference between the two points is acceptable
                if (height - tempHeight < 2)
                {
                    return true;
                }
            }

            return false;
        }
    }
}