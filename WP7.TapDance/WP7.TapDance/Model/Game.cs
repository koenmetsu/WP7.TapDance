using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Threading;
using Microsoft.Phone.Reactive;

namespace WP7.TapDance.Model
{
    public class Game : IGame
    {
        private readonly DispatcherTimer countdownTimer;
        private readonly Stopwatch tapDanceWatch;
        private bool _buttonsCanBeClicked;
        private int countdown;
        private int[] generatedPattern;
        private List<int> tappedButtons;

        public Game()
        {
            tapDanceWatch = new Stopwatch();
            countdownTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.7)};
            ButtonsCanBeClicked = false;
            GameState = GameState.Launched;
        }

        #region IGame Members

        public event EventHandler PlayerLost;
        public event EventHandler PlayerTooFast;
        public event EventHandler<GameWonEventArgs> PlayerWon;
        public event EventHandler<CountdownEventArgs> CountdownTick;
        public event EventHandler WaitForItStarted;
        public event EventHandler TapDanceStarted;
        public event EventHandler ButtonsCanBeClickedChanged;

        public bool ButtonsCanBeClicked
        {
            get { return _buttonsCanBeClicked; }
            set
            {
                _buttonsCanBeClicked = value;
                if (ButtonsCanBeClickedChanged != null)
                {
                    ButtonsCanBeClickedChanged(this, new EventArgs());
                }
            }
        }

        public GameState GameState { get; private set; }

        public void StartCountdown()
        {
            GameState = GameState.CountingDown;
            countdown = 3;
            countdownTimer.Tick -= CountdownTimerOnTick;
            countdownTimer.Tick += CountdownTimerOnTick;
            countdownTimer.Start();
        }

        public int[] GetNewPattern()
        {
            ButtonsCanBeClicked = false;
            ResetTappedButtons();
            generatedPattern = PatternGenerator.Generate(0, 3, 6);
            GameState = GameState.PatternSent;
            return generatedPattern;
        }

        public double GetSecondsPassed()
        {
            return tapDanceWatch.Elapsed.TotalSeconds;
        }

        public void ButtonClicked(int button)
        {
            if (!ButtonsCanBeClicked)
            {
                return;
            }

            if (tapDanceWatch.IsRunning)
            {
                tappedButtons.Add(button);
                if (tappedButtons.Count.Equals(generatedPattern.Length))
                {
                    tapDanceWatch.Stop();
                    if (CheckPattern(tappedButtons.ToArray()))
                    {
                        var score = new Score(tapDanceWatch.Elapsed.TotalSeconds, DateTime.Now);
                        PlayerWon(this, new GameWonEventArgs(score));
                        GameState = GameState.PlayerWon;
                    }
                    else
                    {
                        PlayerLost(this, new EventArgs());
                        GameState = GameState.PlayerWrongCombination;
                    }
                }
            }
            else
            {
                if (countdownTimer.IsEnabled)
                {
                    countdownTimer.Stop();
                }
                PlayerTooFast(this, new EventArgs());
                GameState = GameState.PlayerTooFast;
            }
        }

        #endregion

        private void CountdownTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (countdown == 0)
            {
                GameState = GameState.Active;
                countdownTimer.Stop();
                WaitForItStarted(this, new EventArgs());
                Scheduler.Dispatcher.Schedule(StartTimer, TimeSpan.FromSeconds(new Random().Next(2, 5)));
                ButtonsCanBeClicked = true;
            }
            else
            {
                CountdownTick(this, new CountdownEventArgs(countdown--));
            }
        }

        private void StartTimer()
        {
            if (ButtonsCanBeClicked && countdown == 0 && GameState == GameState.Active)
            {
                tapDanceWatch.Reset();
                tapDanceWatch.Start();
                TapDanceStarted(this, new EventArgs());
            }
        }

        private void ResetTappedButtons()
        {
            tappedButtons = new List<int>();
        }

        private bool CheckPattern(int[] tappedPattern)
        {
            bool correctPattern = true;

            if (tappedPattern.Length > generatedPattern.Length)
            {
                throw new ArgumentOutOfRangeException("tappedPattern", "Tapped pattern is too big");
            }
            for (int i = 0; i < generatedPattern.Length; i++)
            {
                if (tappedPattern[i] != generatedPattern[i])
                {
                    correctPattern = false;
                    break;
                }
            }

            return correctPattern;
        }
    }
}