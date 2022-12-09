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
        internal Boolean start = true;
        internal HashSet<string> history = new HashSet<string>();

        internal Tail()
        {
            // Initialize position and history
            x = 0;
            y = 0;
            history.Add(x.ToString() + "," + y.ToString());
        }

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

            // Add current position to history hashset
            history.Add(x.ToString() + ","+ y.ToString());
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