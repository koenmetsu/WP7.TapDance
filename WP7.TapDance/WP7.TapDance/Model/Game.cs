﻿using System;
using System.Diagnostics;
using System.Windows.Threading;
using System.Collections.Generic;
using Microsoft.Phone.Reactive;

namespace WP7.TapDance.Model
{
    public class Game : IGame
    {
        private readonly Stopwatch tapDanceWatch;
        private readonly DispatcherTimer countdownTimer;
        private int countdown;
        private int[] generatedPattern;
        private List<int> tappedButtons;

        public event EventHandler Stopped;
        public event EventHandler PlayerLost;
        public event EventHandler PlayerWon;
        public event EventHandler<CountdownEventArgs> CountdownTick;
        public event EventHandler WaitForItStarted;
        public event EventHandler TapDanceStarted;

        public bool ButtonsCanBeClicked { get; set; }

        public Game()
        {
            tapDanceWatch = new Stopwatch();
            countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.7) };
            ButtonsCanBeClicked = true;
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
            if (ButtonsCanBeClicked && countdown == 0)
            {
                tapDanceWatch.Reset();
                tapDanceWatch.Start();
                TapDanceStarted(this, new EventArgs());
            }
        }

        public int[] GetNewPattern()
        {
            ResetTappedButtons();
            generatedPattern = PatternGenerator.Generate(0, 3, 6);
            return generatedPattern;
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

        public double GetSecondsPassed()
        {
            return tapDanceWatch.Elapsed.TotalSeconds;
        }

        public void ButtonClicked(int button)
        {
            if(tapDanceWatch.IsRunning)
            {
                tappedButtons.Add(button);
                if (tappedButtons.Count.Equals(generatedPattern.Length))
                {
                    ButtonsCanBeClicked = false;
                    if (CheckPattern(tappedButtons.ToArray())) PlayerWon(this, new EventArgs());
                    else PlayerLost(this, new EventArgs());
                }
            }
            else
            {
                PlayerLost(this, new EventArgs());
            }
        }
    }
}
