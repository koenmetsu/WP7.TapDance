using System;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Media;
using Microsoft.Phone.Reactive;

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
            var game = new ObservableObject();


            //Button1Command = new RelayCommand(game.Button1Clicked, () => game.CanBeClicked);
            //Button2Command = new RelayCommand(game.Button2Clicked, () => game.CanBeClicked);
            //Button3Command = new RelayCommand(game.Button3Clicked, () => game.CanBeClicked);
            //Button4Command = new RelayCommand(game.Button4Clicked, () => game.CanBeClicked);
            //RetryCommand = new RelayCommand(game.Retry, () => true);

            Button1BackColor = new SolidColorBrush(Colors.Black);
            Button2BackColor = new SolidColorBrush(Colors.Black);
            Button3BackColor = new SolidColorBrush(Colors.Black);
            Button4BackColor = new SolidColorBrush(Colors.Black);
            RetryCommand = new RelayCommand(() => LightPattern(new int[] { 0, 2, 3, 1 }), () => true);
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

        public void LightPattern(int[] buttons)
        {
            foreach (int button in buttons)
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
        }

        public void LightButton(SolidColorBrush brush)
        {
            brush.Color = Colors.Orange;
            Scheduler.Dispatcher.Schedule(() => brush.Color = Colors.Black, TimeSpan.FromMilliseconds(700));
        }
    }
}