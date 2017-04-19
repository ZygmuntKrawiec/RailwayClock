using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RailwayClock
{
    /*Contains elements for the MainWindow class*/
    partial class MainWindow
    {        
        ClockMechanism clockMechanism = new ClockMechanism();
        ClockFace clockFace = new ClockFace();
        ClockHandLine secondsClockHand = new ClockHandLine(130, 150, 150, Brushes.Red, 1);
        ClockHandLine minutesClockhand = new ClockHandLine(130, 150, 150, Brushes.Black, 3);
        ClockHandLine hourClockHand = new ClockHandLine(100, 150, 150, Brushes.Black, 6);

        /// <summary>
        /// Joins all clock elements.
        /// </summary>
        private void combineElements()
        {           
            /*Put clock hands on the clock face*/
            clockFace.Background.Children.Add(secondsClockHand.Hand);
            clockFace.Background.Children.Add(minutesClockhand.Hand);
            clockFace.Background.Children.Add(hourClockHand.Hand);

            /*Join clock hands to the clock mechanism*/
            clockMechanism.SecondsHandJunction = secondsClockHand.SetCoordinatesToClockHand;
            clockMechanism.MinutesHandJunction = minutesClockhand.SetCoordinatesToClockHand;
            clockMechanism.HourHandJunction = hourClockHand.SetCoordinatesToClockHand;

            /*Put all elements into the clock casing*/
            clockCasing.AddChild(clockFace.Background);

        }
        /// <summary>
        /// Runs the clock.
        /// </summary>
        private void runClock()
        {
            clockMechanism.Start();
        }


    }
}
