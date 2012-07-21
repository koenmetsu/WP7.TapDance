using System;
using System.Diagnostics;

namespace WP7.TapDance.Model
{
    public class Game : IGame
    {
        private readonly Stopwatch clickedWatch;

        public Game()
        {
            clickedWatch = new Stopwatch();
        }

        public int[] GetNewPattern()
        {
            return new int[] { 1, 0, 2, 3 };
        }

        public double GetSecondsPassed()
        {
            return clickedWatch.Elapsed.TotalSeconds;
        }
    }
}
