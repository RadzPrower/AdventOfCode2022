using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace AdventOfCode_2022
{
    internal class Day05 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(5);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Variable initialization
            var stackProcessing = true;
            var stackLines = new List<string>();
            var stacksSingle = new List<Stack<char>>();
            var stacksMulti = new List<Stack<char>>();

            foreach(var line in lines)
            {
                if (stackProcessing) // This boolean tracks if we are still within the "stack" portion of the input
                {
                    if (line == "") // This is looking for the empty line which delineates the switch from stacks to instructions
                    {
                        stackProcessing = false; // We are at the end of the stack, so next loop will move to next phase

                        for (int i = (stackLines.Count - 1); i >= 0; i--)
                        {
                            for (int j = 1; j < stackLines[i].Length; j += 4)
                            {
                                if(Char.IsDigit(stackLines[i], j)) // Checking if the values in the "boxes" are digits to determine number of stacks
                                {
                                    // Initialize our two sets of stacks
                                    stacksSingle.Add(new Stack<char>());
                                    stacksMulti.Add(new Stack<char>());
                                }
                                else
                                {
                                    if (stackLines[i][j] != ' ') // Check to make sure that we have a box and not empty space
                                    {
                                        // Add our crate to the applicable stack in both our single and multi stack lists
                                        stacksSingle[Convert.ToInt32(stackLines[^1].Substring(j, 1)) - 1].Push(stackLines[i][j]);
                                        stacksMulti[Convert.ToInt32(stackLines[^1].Substring(j, 1)) - 1].Push(stackLines[i][j]);
                                    }
                                }
                            }
                        }
                    }
                    else stackLines.Add(line);
                }
                else // If "stack" portion of input complete, move to the movement phase of the process
                {
                    var movement = line.Split(' ');               // Parse line into array
                    var moves = Convert.ToInt32(movement[1]);     // # of crates to move
                    var from = Convert.ToInt32(movement[3]) - 1;  // Stack to take crates from
                    var to = Convert.ToInt32(movement[5]) - 1;    // Stack to move crates to
                    
                    // Initialize our temporary stack for reversing the pop/push for moving multiple crates at once
                    var tempStack = new Stack<char>();

                    // First pass which moves all crates for single crate sequence and stores multi-crates in temp
                    for (int i = 1; i <= moves; i++)
                    {
                        stacksSingle[to].Push(stacksSingle[from].Pop());

                        tempStack.Push(stacksMulti[from].Pop());
                    }

                    // Takes the temporary stack and pops them all into the intented target stack
                    for (int i = 1; i <= moves; i++)
                    {
                        stacksMulti[to].Push(tempStack.Pop());
                    }
                }
            }

            // Create empty strings and peek at the tops of each stack in both lists to form our sequence
            var topCratesSingle = "";
            var topCratesMulti = "";
            for(int i = 0; i < stacksSingle.Count; i++)
            {
                topCratesSingle += stacksSingle[i].Peek();
                topCratesMulti += stacksMulti[i].Peek();
            }

            // Output results to the terminal
            Console.WriteLine("The sequence of top crates moved one at a time is \"" + topCratesSingle + "\".");
            Console.WriteLine("The sequence of top crates moved as a stack is \"" + topCratesMulti + "\".");

            // Print out the summary of the performance time and return to AoC 2022 main menu
            Summary(watch);
        }
    }
}