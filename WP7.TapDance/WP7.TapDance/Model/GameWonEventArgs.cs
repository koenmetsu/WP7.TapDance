using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WP7.TapDance.Model
{
    public class GameWonEventArgs: EventArgs
    {
        private readonly Score m_newScore;

        public GameWonEventArgs(Score newScore)
        {
            m_newScore = newScore;
        }

        public Score NewScore
        {
            get { return m_newScore; }
        }
    }
}
