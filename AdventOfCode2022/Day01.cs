using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode_2022
{
    internal class Day01 : AoC2022
    {
        internal static void Start()
        {
            var lines = TestPrompt(1);
            var watch = Stopwatch.StartNew();

            var count = 0;
            var totalCalories = 0;
            var elves = new List<Elf>();

            foreach(var line in lines)
            {
                if (line.Equals(""))
                {
                    elves.Add(new Elf(count));
                    count = 0;
                }
                else count += Convert.ToInt32(line);
            }
            elves.Add(new Elf(count));

            var sortedElves = elves.OrderByDescending(x => x.Calories).Take(3).ToList();

            foreach(var elf in sortedElves)
            {
                totalCalories += elf.Calories;
            }

            Console.WriteLine("The top elf is carrying a total of " + sortedElves[0].Calories + " calories.");
            Console.WriteLine("The top three elves are carrying a total of " + totalCalories + " calories.");
            Summary(watch);
        }

        private class Elf
        {
            internal int Calories;

            internal Elf(int calories)
            {
                Calories = calories;
            }
        }
    }
}