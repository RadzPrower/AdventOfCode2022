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
            for (int i = 0; i < lines.Length; i += 3, packetIndex++)
                if (MyComparePackets(lines[i], lines[i + 1])) correctPairs.Add(packetIndex);

            // Print results and performance summary
            Console.WriteLine("The sum of the indices of the correct pairs is " + correctPairs.Sum() + ".");
            Summary(watch);

            // Answer is between 4084 and 4994
        }

        private static bool MyComparePackets(string leftString, string rightString)
        {
            var left = (List<object>)MyParsePacket(leftString)[0];
            var right = (List<object>)MyParsePacket(rightString)[0];

            if (MyCompareLists(left, right) == 1) return true;
            else return false;
        }

        private static int MyCompareLists(List<object> left, List<object> right)
        {
            var maxCount = Math.Max(left.Count, right.Count);

            for (int i = 0; i < maxCount; i++)
            {
                // If left is empty, order is correct
                if (left.ElementAtOrDefault(i) == null) return 1;

                // If right is empty, order is not correct
                if (right.ElementAtOrDefault(i) == null) return -1;

                // If left and right are integers, compare their value
                if (left[i].GetType() == typeof(int)
                    && right[i].GetType() == typeof(int))
                {
                    // If left is smaller than the right, order is correct
                    if ((int)left[i] < (int)right[i]) return 1;

                    // If right is smaller than the left, order is not correct
                    if ((int)left[i] > (int)right[i]) return -1;
                }
                // If left is an integer and right is not, convert left to List and compare
                else if (left[i].GetType() == typeof(int))
                {
                    var tempList = new List<object>();
                    tempList.Add(left[i]);
                    return MyCompareLists(tempList, (List<object>)right[i]);
                }
                // If right is an integer and left is not, convert left to List and compare
                else if (right[i].GetType() == typeof(int))
                {
                    var tempList = new List<object>();
                    tempList.Add(right[i]);
                    return MyCompareLists((List<object>)left[i], tempList);
                }
                // If both left and right are Lists, compare them
                else
                {
                    // Compare the two lists and store results to see if correct or incorrect
                    var result = MyCompareLists((List<object>)left[i], (List<object>)right[i]);

                    // If result was not correct or incorrect, continue
                    if (result != 0) return result;
                }
            }

            return 0;
        }

        private static List<object> MyParsePacket(string input)
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

                    tempList.Add(MyParsePacket(input.Substring(listIndex, listLength - 1)));

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