using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode_2022
{
    internal class Day06 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(6);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Variable initialization
            var packet = 0;
            var message = 0;

            // Find each character segments of the datastream and check for duplicate characters
            for(int i = 0; i <= lines[0].Length - 4; i++)
            {
                // Pull packet marker segment length
                var packetSegment = lines[0].Substring(i, 4);

                // Ensure that enough string remains to pull message marker segment
                var messageSegment = "";
                if (i <= lines[0].Length - 14) messageSegment = lines[0].Substring(i, 14);

                // Utilize IsMarker method to determine if the process can stop and marker value set
                if (IsMarker(packetSegment) && packet == 0)
                {
                    packet = i + 4;
                }

                // Same as above however we must check messageSegment length in case it was not set
                // because we reached the end of the datastream without a marker. This is not going
                // to happen in this scenario given the curated data input, but I wanted to write
                // it as if it were just to satisfy myself.
                if((messageSegment.Length > 0) && IsMarker(messageSegment) && message == 0)
                {
                    message = i + 14;
                }
            }

            // Final console output and call to Summary method to print out performance time
            Console.WriteLine("The first packet marker is detected after " + packet + " characters.");
            Console.WriteLine("The first message marker is detected after " + message + " characters.");
            Summary(watch);
        }

        private static Boolean IsMarker(string segment) // Checks if the input segment is a marker by checking for duplicate characters
        {
            // Initialize our list
            var list = new List<char>();

            // Check each new, incoming letter against the list.
            // If found, this is not a marker and the process can return false.
            // If not found, add the letter to the list and move on until the end of the segment
            foreach(var letter in segment)
            {
                if (list.Contains(letter)) return false;
                else list.Add(letter);
            }

            // No duplicate letters were found, so return true to indicate this is our marker segment
            return true;
        }
    }
}