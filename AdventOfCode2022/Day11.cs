using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_2022
{
    internal class Day11 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(11);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Initalize variables
            var count = 0;
            var monkeys = new Dictionary<int, Monkey>();

            // Parse input
            for(int i = 0; i < lines.Length; i += 7)
            {
                // Determine monkey's parameters
                var items = lines[i + 1].Split(": ")[1].Split(", ");
                var operation = lines[i + 2].Split(" = ")[1].Split(" ");
                var opType = operation[1];
                var opValue = operation[2];
                var testValue = Convert.ToInt32(lines[i + 3].Split(" ").Last());
                var pass = Convert.ToInt32(lines[i + 4].Split(" ").Last());
                var fail = Convert.ToInt32(lines[i + 5].Split(" ").Last());

                // Add monkey to the dictionary
                monkeys.Add(count, new Monkey(opValue, opType, testValue, pass, fail));

                // Add all items to the monkey
                foreach (var item in items)
                {
                    monkeys[count].AddItem(new Item(Convert.ToInt32(item)));
                }

                count++;
            }

            // Find least common multiple (thanks ResetEra)
            var lcm = 1;
            foreach (var monkey in monkeys)
            {
                lcm *= monkey.Value.TestValue;
            }

            // Run through 10000 rounds of "monkey business"
            for (int r = 1; r <= 10000; r++)
            {
                for (int m = 0; m < monkeys.Count; m++)
                {
                    while (monkeys[m].Items.Count > 0)
                    {
                        // Pop the next item
                        var item = monkeys[m].Items.Dequeue();

                        // Have monkey inspect item, changing its "worry" level
                        var receiver = monkeys[m].Inspect(item, lcm);

                        // Pass the item off to the new monkey
                        monkeys[receiver].AddItem(item);
                    }
                }
            }

            // Sort our monkeys by their total number of inspections
            var sortedMonkeys = monkeys.OrderByDescending(m => m.Value.Inspections);

            // Output results and performance summary
            Console.WriteLine("Our level of monkey business after 10000 rounds is " + (sortedMonkeys.ElementAt(0).Value.Inspections * sortedMonkeys.ElementAt(1).Value.Inspections) + "!");
            Summary(watch);
        }
    }

    internal class Monkey
    {
        internal Queue<Item> Items = new Queue<Item>();     // Item queue
        internal string OpValue;                            // Worry operation value
        internal string OpType;                             // Worry operation type
        internal int TestValue;                             // Value to test against (divisor)
        internal int PassMonkey;                            // Monkey # to pass to on pass
        internal int FailMonkey;                            // Monkey # to pass to on fail
        internal long Inspections;                          // Total number of inspections made

        // Create a Monkey item with their parameters initialized
        internal Monkey(string opValue, string opType, int test, int pass, int fail)
        {
            OpValue = opValue;
            OpType = opType;
            TestValue = test;
            PassMonkey = pass;
            FailMonkey = fail;
        }

        // Add an item to a Monkey's queue
        internal void AddItem(Item item)
        {
            Items.Enqueue(item);
        }

        // Inspect an Item
        internal int Inspect(Item item, int lcm)
        {
            long value;

            // Minimize our worry size
            item.Worry %= lcm;

            // Check if value is set or is based on current value
            if (OpValue == "old") value = item.Worry;
            else value = Convert.ToInt16(OpValue);

            // Perform add or multiply action
            switch(OpType)
            {
                case "*":
                    item.Worry *= value;
                    break;
                case "+":
                    item.Worry += value;
                    break;
            }

            // Divide worry by 3 for part 1
            //item.Worry /= 3;

            // Add to inspection count
            Inspections++;

            // Test the item and return monkey # as result
            if (item.Worry % TestValue == 0)
            {
                return PassMonkey;
            }

            return FailMonkey;
        }
    }

    // An Item object...was expecting to need to do more with this...
    internal class Item
    {
        internal long Worry;

        internal Item(long worry)
        {
            Worry = worry;
        }
    }
}