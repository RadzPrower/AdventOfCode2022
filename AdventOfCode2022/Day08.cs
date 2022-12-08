using System;
using System.Diagnostics;

namespace AdventOfCode_2022
{
    internal class Day08 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(8);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Variable initialization
            var width = lines[0].Length;
            var height = lines.Length;
            
            // Create Tree Grid
            var trees = CreateGrid(lines);

            // Check how many trees are visible from the outside
            var results = VisibleCount(trees);

            // Output results and performance summary
            Console.WriteLine("There are " + results.visible + " trees visibile from the outside of the grid.");
            Console.WriteLine("The best possible scenic score is " + results.score + " which is the tree located in " + results.tree + ".");
            Summary(watch);
        }

        // Create a grid of trees based on input data
        private static int[,] CreateGrid(string[] lines)
        {
            var result = new int[lines.Length, lines[0].Length];

            for (int h = 0; h < lines.Length; h++)
            {
                for (int w = 0; w < lines[0].Length; w++)
                {
                    result[h, w] = Convert.ToInt32(lines[h][w].ToString());
                }
            }

            return result;
        }

        // Count visibility and calculate scenic score
        private static (int visible, int score, string tree) VisibleCount(int[,] trees)
        {
            var result = 0;
            var scenicScore = 0;
            var scenicTree = "";

            for (int h = 0; h < trees.GetLength(0); h++)
            {
                for (int w = 0; w < trees.GetLength(1); w++)
                {
                    // Tree variable initialization
                    var treeScore = 0;

                    // Check each direction
                    var north = VisibleFromNorth(trees, h, w);
                    var south = VisibleFromSouth(trees, h, w);
                    var west = VisibleFromWest(trees, h, w);
                    var east = VisibleFromEast(trees, h, w);

                    // If any direction is visible, add to the count
                    if (north.visible || south.visible || west.visible || east.visible) result++;

                    // Calculate scenic score for this tree and store if larger than current highest score
                    treeScore = north.distance * south.distance * west.distance * east.distance;
                    if (treeScore > scenicScore)
                    {
                        scenicScore = treeScore;
                        // Bonus functionality to track and return the specific tree that scores highest
                        scenicTree = "row " + (h + 1) + " and column " + (w + 1);
                    }
                }
            }

            return (result, scenicScore, scenicTree);
        }

        // Check visibility and tree count to the north
        private static (bool visible, int distance) VisibleFromNorth(int[,] trees, int h, int w)
        {
            var distance = 0;

            for(int i = h - 1; i >= 0; i--)
            {
                distance++;
                if (trees[i, w] >= trees[h, w]) return (false, distance);
            }

            return (true, distance);
        }

        // Check visibility and tree count to the south
        private static (bool visible, int distance) VisibleFromSouth(int[,] trees, int h, int w)
        {
            var distance = 0;

            for (int i = h + 1; i < trees.GetLength(0); i++)
            {
                distance++;
                if (trees[i, w] >= trees[h, w]) return (false, distance);
            }

            return (true, distance);
        }

        // Check visibility and tree count to the west
        private static (bool visible, int distance) VisibleFromWest(int[,] trees, int h, int w)
        {
            var distance = 0;

            for (int i = w - 1; i >= 0; i--)
            {
                distance++;
                if (trees[h, i] >= trees[h, w]) return (false, distance);
            }

            return (true, distance);
        }

        // Check visibility and tree count to the east
        private static (bool visible, int distance) VisibleFromEast(int[,] trees, int h, int w)
        {
            var distance = 0;

            for (int i = w + 1; i < trees.GetLength(1); i++)
            {
                distance++;
                if (trees[h, i] >= trees[h, w]) return (false, distance);
            }

            return (true, distance);
        }
    }
}