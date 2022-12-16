using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode_2022
{
    internal class Day16 : AoC2022
    {
        public static class GlobalVar
        {
            public static int depth;
        }

        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(16);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();
            var map = new TunnelMap();

            // Parse input
            foreach(var line in lines)
            {
                map.AddCave(line);
            }

            // Calculate cave connections
            map.CalculateAllCaveConnections();

            // Calculate most efficient route alone
            var tempCaves = map.Caves.ConvertAll(cave => new Cave(cave.Valve, cave.FlowRate, cave.Tunnels, cave.ValveOpen));
            var pressureReleased = map.CalculateEfficientRoute("AA", 1, tempCaves);

            // Calculate most efficient route with elephant
            tempCaves = map.Caves.ConvertAll(cave => new Cave(cave.Valve, cave.FlowRate, cave.Tunnels, cave.ValveOpen));
            var pressureReleasedTogether = map.CalculateEfficientRouteTogether("AA", 4, tempCaves);

            // Output results and performance summary
            Console.WriteLine("The most pressure that can be released alone is " + pressureReleased + ".");
            Summary(watch);
        }
    }

    internal class TunnelMap
    {
        internal List<Cave> Caves = new List<Cave>();
        internal List<Route> ShortestRoutes = new List<Route>();

        public TunnelMap()
        {
        }

        internal void AddCave(string line)
        {
            var words = line.Split(' ');
            var valve = words[1];
            var flowRate = words[4].Split('=')[1];
            flowRate = flowRate[0..(flowRate.Length - 1)];

            var cave = Caves.Find(x => x.Valve == valve);
            if (cave is null) cave = new Cave(valve, Convert.ToInt32(flowRate));
            else cave.FlowRate = Convert.ToInt32(flowRate);
            
            for (int i = 9; i < words.Length; i++)
            {
                words[i] = words[i].Replace(",", "");

                var connectingCave = Caves.Find(x => x.Valve == words[i]);
                if (connectingCave is null) connectingCave = new Cave(words[i]);

                cave.AddConnection(connectingCave.Valve);
            }

            Caves.Add(cave);
        }

        internal void CalculateAllCaveConnections()
        {
            for (int i = 0; i < Caves.Count; i++)
            {
                for (int j = 0; j < Caves.Count; j++)
                {
                    if (i == j) continue;

                    if (Caves[j].ValveOpen || Caves[j].FlowRate == 0) continue;

                    GlobalVar.depth = Int32.MaxValue;
                    var shortestRoute = FindShortestRoute(Caves[i], Caves[j], new List<string>(), 0);

                    ShortestRoutes.Add(new Route(Caves[i].Valve, Caves[j].Valve, shortestRoute));
                }
            }
        }

        private string FindShortestRoute(Cave start, Cave end, List<string> visitedCaves, int depth)
        {
            if (depth >= GlobalVar.depth) return "";

            if (start.Valve == "BB" && end.Valve == "JJ" && visitedCaves.Count == 0) ;

            var result = "";
            var caveCount = Int32.MaxValue;

            visitedCaves.Add(start.Valve);

            if (start.Equals(end))
            {
                GlobalVar.depth = depth;
                return start.Valve;
            }

            if (start.Tunnels.Contains(end.Valve)) return start.Valve + ',' + end.Valve; 

            foreach (var valve in start.Tunnels)
            {
                if (visitedCaves.Contains(valve)) continue;

                var cave = Caves.Find(x => x.Valve == valve);

                var tempVisistedCaves = visitedCaves.ConvertAll(x => x);
                var path = FindShortestRoute(cave, end, tempVisistedCaves, depth + 1);

                if (!path.Contains(end.Valve)) continue;

                var tempCount = CountSegments(path);

                if (tempCount < caveCount)
                {
                    caveCount = tempCount;
                    result = path;
                }
            }

            if (result != "") return start.Valve + ',' + result;
            else return start.Valve;
        }

        private int CountSegments(string path)
        {
            var result = 0;
            var segments = path.Split(',');

            return segments.Length;
        }

        internal int CalculateEfficientRoute(string caveLabel, int minutes, List<Cave> caves)
        {
            if (minutes > 30) return 0;

            var currentCave = Caves.Find(cave => cave.Valve == caveLabel);
            var unopenedValves = caves.FindAll(x => !x.ValveOpen && x.FlowRate > 0);

            var bestPressure = 0;
            foreach (var end in unopenedValves)
            {
                var allRoutes = ShortestRoutes.FindAll(x => x.Start == caveLabel && x.End == end.Valve);
                var route = ShortestRoutes.Find(x => x.Start == caveLabel && x.End == end.Valve);
                if (route is null) continue;

                var tempCaves = caves.ConvertAll(cave => new Cave(cave.Valve, cave.FlowRate, cave.Tunnels, cave.ValveOpen));

                var tempPressure = PotentialPressure(route, minutes, tempCaves);

                var nextMinutes = minutes + route.Length + 1;

                tempPressure += CalculateEfficientRoute(route.End, nextMinutes, tempCaves);

                bestPressure = Math.Max(bestPressure, tempPressure);
            }

            return bestPressure;
        }

        internal int CalculateEfficientRouteTogether(string caveLabel, int minutes, List<Cave> caves)
        {
            var openValves = caves.FindAll(x => !x.ValveOpen && x.FlowRate > 0);

            return -1;
        }

        private int PotentialPressure(Route route, int minutes, List<Cave> cavesTemp)
        {
            var result = 0;
            var PPM = 0;

            for (int i = 1; i <= route.Length; i++)
            {
                minutes++;
                if (minutes > 30) return result;
            }

            // Find valve entry and set flow rate
            var valve = cavesTemp.Find(x => x.Valve == route.End);
            valve.ValveOpen = true;
            PPM += valve.FlowRate;

            // Add minute for opening value
            result += (30 - minutes) * PPM;
            minutes++;
            if (minutes > 30) return 0;

            return result;
        }

        private int TakeRoute(Route route, int minutes, List<Cave> cavesTemp, int pressure)
        {
            // Increase minutes and pressure for each path taken
            for (int i = 1; i <= route.Length; i++)
            {
                minutes++;
                pressure += PressureReleased(cavesTemp);
                if (minutes > 30) return pressure;
            }

            // Find valve entry and open
            var valve = cavesTemp.Find(x => x.Valve == route.End);
            valve.ValveOpen = true;

            // Time taken to open valve
            minutes++;
            pressure += PressureReleased(cavesTemp);

            return pressure;
        }

        private int PressureReleased(List<Cave> cavesTemp)
        {
            var result = 0;

            var openValves = cavesTemp.FindAll(x => x.ValveOpen);
            foreach (var valve in openValves)
            {
                result += valve.FlowRate;
            }

            return result;
        }
    }

    internal class Route
    {
        internal string Start;
        internal string End;
        internal int Length;
        internal string[] Path;

        public Route(string start, string end, string path)
        {
            Start = start;
            End = end;
            Path = path.Split(',');
            Length = Path.Length - 1;
        }
    }

    internal class Cave
    {
        internal string Valve;
        internal int FlowRate;
        internal List<string> Tunnels = new List<string>();
        internal bool ValveOpen = false;
        internal bool Visisted = false;

        public Cave(string valve)
        {
            Valve = valve;
        }

        public Cave(string valve, int flowRate)
        {
            Valve = valve;
            FlowRate = flowRate;
        }

        public Cave(string valve, int flowRate, List<string> tunnels, bool valveOpen)
        {
            Valve = valve;
            FlowRate = flowRate;
            Tunnels = tunnels;
            ValveOpen = valveOpen;
        }

        internal void AddConnection(string label)
        {
            Tunnels.Add(label);
        }
    }
}