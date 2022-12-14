using System;
using System.Diagnostics;

namespace AdventOfCode_2022
{
    internal class Day25 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(14);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Output results and performance summary
            Summary(watch);
        }
    }
}