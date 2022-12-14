using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2022
{
    class AoC2022
    {
        [STAThread]
        static void Main()
        {
            int day = 26;
            Console.Clear();
            Console.WriteLine("Which day's assignment do you wish to run?");
            Console.WriteLine("    1. Calorie Counting".PadRight(35) + "14: ???");
            Console.WriteLine("    2. Rock Paper Scissors".PadRight(35) + "15: ???");
            Console.WriteLine("    3. Rucksack Reorganization".PadRight(35) + "16: ???");
            Console.WriteLine("    4. Camp Cleanup".PadRight(35) + "17: ???");
            Console.WriteLine("    5. Supply Stacks".PadRight(35) + "18: ???");
            Console.WriteLine("    6. Tuning Trouble".PadRight(35) + "19: ???");
            Console.WriteLine("    7: No Space Left On Device".PadRight(35) + "20: ???");
            Console.WriteLine("    8: Treetop Tree House".PadRight(35) + "21: ???");
            Console.WriteLine("    9: Rope Bridge".PadRight(35) + "22: ???");
            Console.WriteLine("   10: Cathode-Ray Tube".PadRight(35) + "23: ???");
            Console.WriteLine("   11: Monkey in the Middle".PadRight(35) + "24: ???");
            Console.WriteLine("   12: Hill Climbing Algorithm".PadRight(35) + "25: ???");
            Console.WriteLine("   13: Distress Signal".PadRight(35) + " 0: Quit");
            Console.WriteLine();
            Console.Write("Selection: ");
            try
            {
                day = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Invalid input! Please select a numerical value from above.");
                Console.ReadLine();
                Main();
                return;
            }
            switch (day)
            {
                case 0:
                    Console.WriteLine("Thank you for participating in Advent of Code 2022!");
                    Console.ReadLine(); //To hold for final user input to close debug; can be removed in final, command line version
                    break;
                case 1:
                    Day01.Start();
                    break;
                case 2:
                    Day02.Start();
                    break;
                case 3:
                    Day03.Start();
                    break;
                case 4:
                    Day04.Start();
                    break;
                case 5:
                    Day05.Start();
                    break;
                case 6:
                    Day06.Start();
                    break;
                case 7:
                    Day07.Start();
                    break;
                case 8:
                    Day08.Start();
                    break;
                case 9:
                    Day09.Start();
                    break;
                case 10:
                    Day10.Start();
                    break;
                case 11:
                    Day11.Start();
                    break;
                case 12:
                    Day12.Start();
                    break;
                case 13:
                    Day13.Start();
                    break;
                case 14:
                    Day14.Start();
                    break;
                case 15:
                    Day15.Start();
                    break;
                case 16:
                    Day16.Start();
                    break;
                case 17:
                    Day17.Start();
                    break;
                case 18:
                    Day18.Start();
                    break;
                case 19:
                    Day19.Start();
                    break;
                case 20:
                    Day20.Start();
                    break;
                case 21:
                    Day21.Start();
                    break;
                case 22:
                    Day22.Start();
                    break;
                case 23:
                    Day23.Start();
                    break;
                case 24:
                    Day24.Start();
                    break;
                case 25:
                    Day25.Start();
                    break;
                default:
                    Console.WriteLine("Invalid input! Please select a value between 0 - 25.");
                    Console.ReadLine();
                    Main();
                    break;
            }
        }

        internal static string[] TestPrompt(int day)
        {
            string[] lines;

            Console.Clear();
            Console.Write("Do you wish to run a test? (Y/N): ");

            if (Console.ReadLine().ToUpper() == "Y")
            {
                Console.WriteLine();
                Console.WriteLine($"Importing data from Day {day} Test.txt...");

                lines = System.IO.File.ReadAllLines($"Day {day} Test.txt");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"Importing data from Day {day} Input.txt...");

                lines = System.IO.File.ReadAllLines($"Day {day} Input.txt");
            }

            Console.WriteLine();
            return lines;
        }

        internal static void Summary(System.Diagnostics.Stopwatch watch)
        {
            watch.Stop();
            double time = watch.ElapsedTicks;
            double ms = time / TimeSpan.TicksPerMillisecond;
            double sec = time / TimeSpan.TicksPerSecond;

            Console.WriteLine($"Time elapsed: {ms}ms ({sec} sec)");
            Console.WriteLine();
            Console.Write("Press enter to continue");
            Console.ReadLine();
            Main();
        }

        internal static int Sum(int input)
        {
            var sum = 0;
            for (int i = 1; i <= input; i++)
            {
                sum += i;
            }
            return sum;
        }
    }
}
