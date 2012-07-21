using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace WP7.TapDance.Model
{
    public class Game : IGame
    {
        private readonly Stopwatch clickFastWatch;
        private readonly DispatcherTimer countdownTimer;
        private int countdown;

        public event EventHandler Stopped;
        public event EventHandler PlayerLost;
        public event EventHandler<GameWonEventArgs> PlayerWon;
        public event EventHandler<CountdownEventArgs> CountdownTick;
        public event EventHandler WaitForItStarted;
        public event EventHandler ClickFastStarted;

        public Game()
        {
            clickFastWatch = new Stopwatch();
            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.7) };
        }

        public void StartCountdown()
        {
            countdown = 3;
            countdownTimer.Tick -= CountdownTimerOnTick;
            countdownTimer.Tick += CountdownTimerOnTick;
            countdownTimer.Start();
        }

        private void CountdownTimerOnTick(object sender, EventArgs eventArgs)
        {
            if(countdown == 0)
            {
                countdownTimer.Stop();
                WaitForItStarted(this, new EventArgs());   
            }
            else
            {
                CountdownTick(this, new CountdownEventArgs(countdown--));    
            }
        }

        public int[] GetNewPattern()
        {
            return PatternGenerator.Generate(0, 3, 6);
        }

        public double GetSecondsPassed()
        {
            return clickFastWatch.Elapsed.TotalSeconds;
        }

        public void ButtonClicked(int button)
        {
            if(clickFastWatch.IsRunning)
            {
                var score = new Score(clickFastWatch.Elapsed.TotalSeconds, DateTime.Now);
                PlayerWon(this, new GameWonEventArgs(score));
            }
            else
            {
                PlayerLost(this, new EventArgs());
            }
        }

        public bool ButtonsCanBeClicked
        {
            get { return true; }
        }
    }
}
