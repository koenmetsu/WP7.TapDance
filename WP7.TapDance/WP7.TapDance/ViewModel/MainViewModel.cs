using System;
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
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly TimeSpan buttonLightingTime = TimeSpan.FromMilliseconds(700);
        private readonly int[] pattern;
        private readonly DispatcherTimer patternTimer;
        private int currentButtonInPattern;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            IGame game = null;
            patternTimer = new DispatcherTimer();
            patternTimer.Tick += PatternTimerOnTick;
            patternTimer.Interval = buttonLightingTime;
            pattern = new int[4] {0, 2, 3, 1}; //game.GetNewPattern();
            currentButtonInPattern = 0;
            patternTimer.Start();

            //Button1Command = new RelayCommand(() => game.ButtonClicked(0), () => game.ButtonsCanBeClicked);
            //Button2Command = new RelayCommand(() => game.ButtonClicked(1), () => game.ButtonsCanBeClicked);
            //Button3Command = new RelayCommand(() => game.ButtonClicked(2), () => game.ButtonsCanBeClicked);
            //Button4Command = new RelayCommand(() => game.ButtonClicked(3), () => game.ButtonsCanBeClicked);
            //RetryCommand = new RelayCommand(game.Retry, () => true);

            Button1BackColor = new SolidColorBrush(Colors.Black);
            Button2BackColor = new SolidColorBrush(Colors.Black);
            Button3BackColor = new SolidColorBrush(Colors.Black);
            Button4BackColor = new SolidColorBrush(Colors.Black);
            //RetryCommand = new RelayCommand(() => LightPattern(new int[] {0, 2, 3, 1}), () => true);
        }

        public RelayCommand Button1Command { get; set; }
        public RelayCommand Button2Command { get; set; }
        public RelayCommand Button3Command { get; set; }
        public RelayCommand Button4Command { get; set; }
        public RelayCommand RetryCommand { get; set; }

        public SolidColorBrush Button1BackColor { get; set; }
        public SolidColorBrush Button2BackColor { get; set; }
        public SolidColorBrush Button3BackColor { get; set; }
        public SolidColorBrush Button4BackColor { get; set; }

        private void PatternTimerOnTick(object sender, EventArgs eventArgs)
        {
            LightPattern(pattern[currentButtonInPattern++]);
            if (currentButtonInPattern == 4)
            {
                patternTimer.Stop();
            }
        }

        public void LightPattern(int button)
        {
            switch (button)
            {
                case 0:
                    LightButton(Button1BackColor);
                    break;
                case 1:
                    LightButton(Button2BackColor);
                    break;
                case 2:
                    LightButton(Button3BackColor);
                    break;
                case 3:
                    LightButton(Button4BackColor);
                    break;
            }
        }

        public void LightButton(SolidColorBrush brush)
        {
            brush.Color = Colors.Orange;
            Scheduler.Dispatcher.Schedule(() => brush.Color = Colors.Black, buttonLightingTime);
        }
    }
}