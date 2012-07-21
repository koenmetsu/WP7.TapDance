using System;

namespace WP7.TapDance.Model
{
    public interface IGame
    {
        event EventHandler PlayerLost;
        event EventHandler PlayerTooFast;
        event EventHandler<GameWonEventArgs> PlayerWon;
        event EventHandler<CountdownEventArgs> CountdownTick;
        event EventHandler WaitForItStarted;
        event EventHandler TapDanceStarted;
        
        int[] GetNewPattern();
        void StartCountdown();
        double GetSecondsPassed();

        void ButtonClicked(int button);
        bool ButtonsCanBeClicked { get; }


    }
}