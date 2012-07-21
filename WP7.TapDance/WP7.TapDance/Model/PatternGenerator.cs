using System;
namespace WP7.TapDance.Model
{
    public static class PatternGenerator
    {
        public static int[] Generate(int minimum, int maximum, int patternSize)
        {
            var randomizer = new Random();
            var pattern = new int[patternSize];
            for(int i =0; i < patternSize; i++)
            {
                pattern[i] = randomizer.Next(minimum, maximum);
            }

            return pattern;
        }
    }
}