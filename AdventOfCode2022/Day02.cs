using System;
using System.Diagnostics;

namespace AdventOfCode_2022
{
    internal class Day02 : AoC2022
    {
        internal static void Start()
        {
            var lines = TestPrompt(2);
            var watch = Stopwatch.StartNew();

            var totalScore1 = 0;
            var totalScore2 = 0;
            foreach(var line in lines)
            {
                var hands = line.Split(' ');

                var opponent = new Hand(hands[0]);
                var player = new Hand(hands[1]);

                totalScore1 += player.PlayScore(opponent);
                totalScore2 += opponent.RoundScore(hands[1]);
            }

            Console.WriteLine("Using the strategy guide as what hand to play will result in a score of " + totalScore1 + " points.");
            Console.WriteLine("Using the strategy guide as whether to win, lose, or draw will result in a score of " + totalScore2 + " points.");
            Summary(watch);
        }
    }

    internal class Hand
    {
        private readonly int Play;

        public Hand(string play)
        {
            switch (play)
            {
                case "A":
                case "X":
                    Play = 1;
                    break;
                case "B":
                case "Y":
                    Play = 2;
                    break;
                case "C":
                case "Z":
                    Play = 3;
                    break;
            }
        }

        public int PlayScore(Hand opponent)
        {
            var difference = Play - opponent.Play;

            if((Play == 1) && (opponent.Play == 3))
            {
                return Play + 6;
            }
            else if ((Play == 3) && (opponent.Play == 1))
            {
                return Play;
            }

            if (difference > 0) return Play + 6;
            else if (difference == 0) return Play + 3;

            return Play;
        }

        internal int RoundScore(string hand)
        {
            switch (hand)
            {
                case "X":
                    return Lose();
                case "Y":
                    return Draw();
                case "Z":
                    return Win();
            }

            return 0;
        }

        private int Win()
        {
            var play = Play + 1;
            if (play > 3) play = 1; 
            return 6 + play;
        }

        private int Draw()
        {
            return 3 + Play;
        }

        private int Lose()
        {
            var play = Play - 1;
            if (play < 1) play = 3;
            return play;
        }
    }
}