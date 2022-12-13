using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode_2022
{
    internal class Day13 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(13);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Variable initialization
            var packetIndex = 1;
            var correctPairs = new List<int>();

            // Parse input
            for(int i = 0; i < lines.Length; i += 3, packetIndex++)
                if (ComparePackets(lines[i], lines[i + 1])) correctPairs.Add(packetIndex);

            // Print results and performance summary
            Console.WriteLine("The sume of the indices of the correct pairs is " + correctPairs.Sum() + ".");
            Summary(watch);
        }

        private static bool ComparePackets(string leftString, string rightString)
        {
            var left = (List<object>)ParsePacket(leftString)[0];
            var right = (List<object>)ParsePacket(rightString)[0];

            if (CompareLists(left, right) == 1) return true;
            else return false;
        }

        private static int CompareLists(List<object> left, List<object> right)
        {
            var maxCount = left.Count;
            if (maxCount < right.Count) maxCount = right.Count;

            if (left == right) return -1;
            else
            {
                for (int i = 0; i < maxCount; i++)
                {
                    if (left.Count <= i) return 1;
                    if (right.Count <= i) return -1;

                    if (left[i].GetType() == typeof(int)
                        && right[i].GetType() == typeof(int))
                    {
                        if ((int)left[i] < (int)right[i]) return 1;
                        if ((int)left[i] > (int)right[i]) return -1;
                    }
                    else if (left[i].GetType() == typeof(int)
                        && right[i].GetType() == typeof(List<object>))
                    {
                        var tempList = new List<object>();
                        tempList.Add(left[i]);
                        return CompareLists(tempList, (List<object>)right[i]);
                    }
                    else if (left[i].GetType() == typeof(List<object>)
                        && right[i].GetType() == typeof(int))
                    {
                        var tempList = new List<object>();
                        tempList.Add(right[i]);
                        return CompareLists((List<object>)left[i], tempList);
                    }
                    else
                    {
                        var result = CompareLists((List<object>)left[i], (List<object>)right[i]);
                        if (result != 0) return result;
                    }
                }
            }

            return 0;
        }

        private static List<object> ParsePacket(string input)
        {
            var tempList = new List<object>();
            var currentIndex = 0;
            var endIndex = input.Length;

            while (currentIndex < endIndex)
            {
                var currentChar = input[currentIndex];

                if (currentChar == '[')
                {
                    var listIndex = currentIndex + 1;
                    var listLength = 0;
                    var listDepth = 1;

                    currentIndex++;

                    while (listDepth > 0)
                    {
                        currentChar = input[currentIndex++];

                        // Nest deeper until a closing bracket is found
                        if (currentChar == '[') listDepth++;       // Another opening bracket means we need to go down one nest
                        else if (currentChar == ']') listDepth--;  // A closing bracket means the end of the current list

                        // Our list of integers (in string format) is getting longer
                        listLength++;
                    }

                    tempList.Add(ParsePacket(input.Substring(listIndex, listLength - 1)));

                    currentIndex++;
                }
                else
                {
                    // Parse the int values separately from lists
                    var listIndex = currentIndex;
                    var listLength = 0;

                    // Loop through to each comma to find entire value entry
                    while (currentIndex < endIndex && char.IsDigit(input[currentIndex++]))
                    {
                        listLength++;
                    }

                    if (listLength > 0)
                    {
                        // Add our full integer entry
                        tempList.Add(Convert.ToInt32(input.Substring(listIndex, listLength)));
                    }
                }
            }

            return tempList;
        }
    }
}