using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode_2022
{
    internal class Day09 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(9);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Variable initialization
            var head = new Head();
            var tails = new List<Tail>();
            for(int i = 0; i < 9; i++)
            {
                tails.Add(new Tail());
            }

            // Move the head and tail for each command
            foreach(var line in lines)
            {
                // Initialize variables for each command
                var cmd = line.Split(' ');
                var direction = cmd[0];
                var distance = Convert.ToInt32(cmd[1]);

                // Repeat steps for each unit of distance
                for (int i = 1; i <= distance; i++)
                {
                    head.Move(direction);
                    for (int k = 0; k < 9; k++)
                    {
                        // Special case for first tail knot since it needs to follow head rather than previous tail knot
                        if (k == 0) tails[k].Move(head);
                        else tails[k].Move(tails[k - 1]);
                    }
                }
            }

            // Output results and performance summary
            tails[0].PrintHistory();
            Console.WriteLine();
            tails[8].PrintHistory();
            Console.WriteLine("The second knot of the rope visits " + tails[0].history.Count + " distinct points.");
            Console.WriteLine("The final knot of the rope visits " + tails[8].history.Count + " distinct points.");
            Summary(watch);
        }
    }

    internal class Head : Knot
    {
        internal Head()
        {
            x = 0;
            y = 0;
        }

        internal void Move(string direction)
        {
            switch(direction)
            {
                case "U":
                    y++;
                    break;
                case "D":
                    y--;
                    break;
                case "R":
                    x++;
                    break;
                case "L":
                    x--;
                    break;
            }
        }
    }

    internal class Tail : Knot
    {
        internal HashSet<string> history = new HashSet<string>();
        internal int maxX = 0;
        internal int minX = 0;
        internal int maxY = 0;
        internal int minY = 0;

        internal Tail()
        {
            // Initialize position and history
            x = 0;
            y = 0;
            history.Add(x.ToString() + "," + y.ToString());
        }

        // Move the tail knot towards the input knot
        internal void Move(Knot knot)
        {
            // Find difference between head and tail positions
            var xDif = knot.X - x;
            var yDif = knot.Y - y;

            // If difference is farther than 1 space away, move tail
            if (Math.Abs(xDif) > 1 || Math.Abs(yDif) > 1)
            {
                x += Math.Sign(xDif);
                y += Math.Sign(yDif);
            }

            // Store min/max values for printing
            if (x > maxX) maxX = x;
            if (x < minX) minX = x;
            if (y > maxY) maxY = y;
            if (y < minY) minY = y;

            // Add current position to history hashset
            history.Add(x.ToString() + ","+ y.ToString());
        }

        // Print grid for the tail's history
        internal void PrintHistory()
        {
            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (x == 0 && y == 0) Console.Write("S"); // Always print 0,0 as starting point "S"
                    else if (history.Contains(x + "," + y)) Console.Write("#"); // If tail visited this point, print "#"
                    else Console.Write("."); // Fill the remainder of the grid with "."
                }
                Console.WriteLine();
            }
        }
    }

    internal class Knot
    {
        internal int x;
        internal int y;

        internal int X
        {
            get { return x; }
            set { x = value; }
        }

        internal int Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}