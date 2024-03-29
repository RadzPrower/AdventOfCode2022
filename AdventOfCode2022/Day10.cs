﻿using System;
using System.Diagnostics;

namespace AdventOfCode_2022
{
    internal class Day10 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(10);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Variable initialization
            var history = new int[241];
            history[0] = 1;
            var signalSum = 0;
            var cycle = 1;
            var output = "";

            // Process each command
            foreach(var line in lines)
            {
                var cmd = line.Split(" ");
                
                switch (cmd[0])
                {
                    case "addx":
                        history[cycle] = history[cycle - 1];
                        var scanline = PrintScanline();
                        FindOutput();

                        cycle++;
                        if (cycle > 240) break;

                        history[cycle] = history[cycle - 1];
                        FindOutput();
                        history[cycle] = history[cycle - 1] + Convert.ToInt32(cmd[1]);
                        scanline = PrintScanline();
                        break;
                    case "noop":
                        history[cycle] = history[cycle - 1];
                        FindOutput();
                        break;
                }

                cycle++;

                if (cycle > 240) break;
            }

            // Find sum of requested signal strengths (20, 60, 100, 140, 180, 220)
            for(int i = 20; i <= 220; i += 40)
            {
                signalSum += i * history[i - 1];
            }

            // Output results and performance summary
            Console.WriteLine("The sum of the six signal strengths is " + signalSum + ".");
            Console.WriteLine("This is the image generated by the signals:");
            for(int i = 0; i <= 200; i += 40) Console.WriteLine(output.Substring(i, 40));
            Summary(watch);

            void FindOutput()
            {
                if ((cycle - 1) % 40 >= (history[cycle] - 1) && (cycle - 1) % 40 <= (history[cycle] + 1))
                {
                    output += "#";
                }
                else output += ".";
            }

            string PrintScanline()
            {
                var scanline = "";
                for(int i = 0; i < 40; i++)
                {
                    if (i >= history[cycle] - 1 && i <= history[cycle] + 1) scanline += "#";
                    else scanline += ".";
                }
                return scanline;
            }
        }
    }
}