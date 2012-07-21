using System;

namespace WP7.TapDance.Model
{
    public class Score
    {
        public double Seconds { get; set; }
        public DateTime Date { get; set; }

        public Score()
        {

        }

        public Score(double seconds, DateTime date)
        {
            Seconds = seconds;
            Date = date;
        }
    }
}
