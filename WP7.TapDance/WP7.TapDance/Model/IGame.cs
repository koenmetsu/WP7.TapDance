using System;

namespace WP7.TapDance.Model
{
    public interface IGame
    {
        event EventHandler Stopped;
        event EventHandler PlayerLost;
        event EventHandler PlayerWon;
        event EventHandler<CountdownEventArgs> CountdownTick;
        event EventHandler WaitForItStarted;
        event EventHandler ClickFastStarted;
        
        int[] GetNewPattern();
        void StartCountdown();
        double GetSecondsPassed();

        void ButtonClicked(int button);
        bool ButtonsCanBeClicked { get; }


    }
}