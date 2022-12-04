using System;
using System.Diagnostics;

namespace AdventOfCode_2022
{
    internal class Day04 : AoC2022
    {
        internal static void Start()
        {
            var lines = TestPrompt(4);
            var watch = Stopwatch.StartNew();

            var contains = 0;
            var overlaps = 0;

            foreach(var line in lines)
            {
                var areas = line.Split(',');

                var area1 = new CleaningArea(areas[0]);
                var area2 = new CleaningArea(areas[1]);

                if (area1.Contains(area2)) contains++;
                else if (area2.Contains(area1)) contains++;

                if (area1.Overlaps(area2)) overlaps++;
            }

            Console.WriteLine("There are " + contains + " pairs that one range fully covers the other.");
            Console.WriteLine("There are " + overlaps + " pairs which overlap.");
            Summary(watch);
        }
    }

    internal class CleaningArea
    {
        private int Start;
        private int End;

        public CleaningArea(string input)
        {
            var points = input.Split("-");

            Start = Convert.ToInt32(points[0]);
            End = Convert.ToInt32(points[1]);
        }

        public Boolean Contains(CleaningArea area)
        {
            if (Start <= area.Start && End >= area.End) return true;

            return false;
        }

        public Boolean Overlaps(CleaningArea area)
        {
            if (area.Start >= Start && area.Start <= End) return true;
            else if (Start >= area.Start && Start <= area.End) return true;

            return false;
        }
    }
}