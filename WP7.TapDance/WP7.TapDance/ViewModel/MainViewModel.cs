using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;
using WP7.TapDance.Model;

namespace WP7.TapDance.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly TimeSpan buttonLightingTime = TimeSpan.FromMilliseconds(200);
        private readonly IGame game;
        private readonly DispatcherTimer patternTimer;
        private readonly ScoreStorage scoreStorage;
        private int currentButtonInPattern;
        private string m_button1Text;
        private string m_button2Text;
        private string m_button3Text;
        private string m_button4Text;
        private string m_startRetryText;
        private int[] pattern;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            scoreStorage = new ScoreStorage();
            HighScores = new ObservableCollection<Score>(scoreStorage.GetScores());
            game = new Game();
            game.PlayerLost += GameOnPlayerLost;
            game.PlayerTooFast += GameOnPlayerTooFast;
            game.PlayerWon += GameOnPlayerWon;
            game.CountdownTick += GameOnCountdownTick;
            game.WaitForItStarted += GameOnWaitForItStarted;
            game.TapDanceStarted += GameOnTapDanceStarted;
            game.ButtonsCanBeClickedChanged += GameOnButtonsCanBeClickedChanged;

            patternTimer = new DispatcherTimer();
            patternTimer.Tick += PatternTimerOnTick;
            patternTimer.Interval = buttonLightingTime;

            Button1Command = new RelayCommand(() => game.ButtonClicked(0), () => game.ButtonsCanBeClicked);
            Button2Command = new RelayCommand(() => game.ButtonClicked(1), () => game.ButtonsCanBeClicked);
            Button3Command = new RelayCommand(() => game.ButtonClicked(2), () => game.ButtonsCanBeClicked);
            Button4Command = new RelayCommand(() => game.ButtonClicked(3), () => game.ButtonsCanBeClicked);
            StartRetryCommand = new RelayCommand(() => StartNewPattern(game.GetNewPattern()), () => true);

            Button1BackColor = new SolidColorBrush(Colors.Black);
            Button2BackColor = new SolidColorBrush(Colors.Black);
            Button3BackColor = new SolidColorBrush(Colors.Black);
            Button4BackColor = new SolidColorBrush(Colors.Black);
            StartRetryText = "Start";
            SetNumberButtonsToDefault();
        }

        public RelayCommand Button1Command { get; set; }
        public RelayCommand Button2Command { get; set; }
        public RelayCommand Button3Command { get; set; }
        public RelayCommand Button4Command { get; set; }
        public RelayCommand StartRetryCommand { get; set; }

        public string StartRetryText
        {
            get { return m_startRetryText; }
            set
            {
                m_startRetryText = value;
                RaisePropertyChanged(() => StartRetryText);
            }
        }

        public SolidColorBrush Button1BackColor { get; set; }
        public SolidColorBrush Button2BackColor { get; set; }
        public SolidColorBrush Button3BackColor { get; set; }
        public SolidColorBrush Button4BackColor { get; set; }

        public string Button1Text
        {
            get { return m_button1Text; }
            set
            {
                m_button1Text = value;
                RaisePropertyChanged(() => Button1Text);
            }
        }

        public string Button2Text
        {
            get { return m_button2Text; }
            set
            {
                m_button2Text = value;
                RaisePropertyChanged(() => Button2Text);
            }
        }

        public string Button3Text
        {
            get { return m_button3Text; }
            set
            {
                m_button3Text = value;
                RaisePropertyChanged(() => Button3Text);
            }
        }

        public string Button4Text
        {
            get { return m_button4Text; }
            set
            {
                m_button4Text = value;
                RaisePropertyChanged(() => Button4Text);
            }
        }

        public ObservableCollection<Score> HighScores { get; private set; }

        private void GameOnButtonsCanBeClickedChanged(object sender, EventArgs eventArgs)
        {
            Button1Command.RaiseCanExecuteChanged();
            Button2Command.RaiseCanExecuteChanged();
            Button3Command.RaiseCanExecuteChanged();
            Button4Command.RaiseCanExecuteChanged();
        }

        private void GameOnTapDanceStarted(object sender, EventArgs eventArgs)
        {
            SetAllButtons("Go!");
        }

        private void GameOnWaitForItStarted(object sender, EventArgs eventArgs)
        {
            SetAllButtons("Wait for it...");
        }


        private void GameOnCountdownTick(object sender, CountdownEventArgs eventArgs)
        {
            if (game.GameState == GameState.CountingDown)
            {
                SetAllButtons(eventArgs.SecondsToGo.ToString(CultureInfo.InvariantCulture));
                Color color = Colors.LightGray;
                LightButton(Button1BackColor, color);
                LightButton(Button2BackColor, color);
                LightButton(Button3BackColor, color);
                LightButton(Button4BackColor, color);
            }
        }

        private void GameOnPlayerWon(object sender, GameWonEventArgs eventArgs)
        {
            SetAllButtons("You won!", Colors.Blue);
            // this could probably be better ( observablecollection is of no use here )
            scoreStorage.AddScore(eventArgs.NewScore);
            RefreshHighScores();
        }

        private void RefreshHighScores()
        {
            HighScores.Clear();
            foreach (Score highScore in scoreStorage.GetScores())
            {
                HighScores.Add(highScore);
            }
        }

        private void GameOnPlayerLost(object sender, EventArgs eventArgs)
        {
            SetAllButtons("Wrong combo...", Colors.Red);
        }

        private void GameOnPlayerTooFast(object sender, EventArgs eventArgs)
        {
            SetAllButtons("Aaaah, too fast!", Colors.Red);
            if (patternTimer.IsEnabled)
            {
                patternTimer.Stop();
            }
        }

        public void SetAllButtons(string text)
        {
            Button1Text = text;
            Button2Text = text;
            Button3Text = text;
            Button4Text = text;
        }

        public void SetAllButtons(string text, Color backColor)
        {
            SetAllButtons(text);
            SetAllButtonsBackColors(backColor);
        }

        public void SetAllButtonsBackColors(Color backColor)
        {
            Button1BackColor.Color = backColor;
            Button2BackColor.Color = backColor;
            Button3BackColor.Color = backColor;
            Button4BackColor.Color = backColor;
        }

        public void SetNumberButtonsToDefault()
        {
            Button1Text = "1";
            Button2Text = "2";
            Button3Text = "3";
            Button4Text = "4";
        }

        public void StartNewPattern(int[] newPattern)
        {
            patternTimer.Stop();
            StartRetryText = "Retry";
            SetAllButtonsBackColors(Colors.Black);
            SetNumberButtonsToDefault();
            pattern = newPattern;
            currentButtonInPattern = 0;
            game.ButtonsCanBeClicked = true;
            Scheduler.Dispatcher.Schedule(() => patternTimer.Start(), buttonLightingTime);
        }

        private void PatternTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (game.GameState == GameState.PatternSent)
            {
                LightPattern(pattern[currentButtonInPattern++]);
                if (currentButtonInPattern == pattern.Count())
                {
                    patternTimer.Stop();
                    Scheduler.Dispatcher.Schedule(() => game.StartCountdown(), buttonLightingTime);
                }
            }
        }

        public void LightPattern(int button)
        {
            Color color = Colors.Orange;
            switch (button)
            {
                case 0:
                    LightButton(Button1BackColor, color);
                    break;
                case 1:
                    LightButton(Button2BackColor, color);
                    break;
                case 2:
                    LightButton(Button3BackColor, color);
                    break;
                case 3:
                    LightButton(Button4BackColor, color);
                    break;
            }
        }

        public void LightButton(SolidColorBrush brush, Color color)
        {
            brush.Color = color;
            Scheduler.Dispatcher.Schedule(UnLightButton(brush),
                                          buttonLightingTime.Add(TimeSpan.FromMilliseconds(-100)));
        }

        private Action UnLightButton(SolidColorBrush brush)
        {
            return () =>
                       {
                           if (game.GameState == GameState.CountingDown || game.GameState == GameState.PatternSent)
                           {
                               brush.Color = Colors.Black;
                           }
                       };
        }
    }
}