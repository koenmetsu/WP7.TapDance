using System;

namespace WP7.TapDance.Model
{
    public interface IGame
    {
        int[] GetNewPattern();
        double GetSecondsPassed();
    }
}