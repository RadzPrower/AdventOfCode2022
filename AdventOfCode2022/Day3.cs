using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode_2022
{
    internal class Day3 : AoC2022
    {
        internal static void Start()
        {
            var lines = TestPrompt(3);
            var watch = Stopwatch.StartNew();
            var letters = new LetterTable();
            var priorities = 0;
            var badges = 0;
            var group = new List<string>();

            foreach(var line in lines)
            {
                var comp1 = line.Substring(0, line.Length / 2);
                var comp2 = line.Substring(line.Length / 2, line.Length / 2);
                group.Add(line);

                var duplicate = comp1.Intersect(comp2);

                priorities += letters.Table[duplicate.First()];

                if (group.Count >= 3)
                {
                    var duplicate1 = group[0].Intersect(group[1]);
                    var duplicate2 = group[1].Intersect(group[2]);
                    var duplicate3 = duplicate1.Intersect(duplicate2);

                    badges += letters.Table[duplicate3.First()];

                    group.Clear();
                }
            }

            Console.WriteLine("The sum of the priorities of the duplicate items is " + priorities);
            Console.WriteLine("The sum of badge priorities is " + badges);
            Summary(watch);
        }
    }

    internal class LetterTable
    {
        public readonly Dictionary<char, int> Table = new Dictionary<char, int>();

        public LetterTable()
        {
            for(int i = 1; i <= 26; i++)
            {
                Table.Add(Convert.ToChar(96 + i), i);
            }

            for(int i = 1; i <= 26; i++)
            {
                Table.Add(Convert.ToChar(64 + i), 26 + i);
            }
        }
    }
}