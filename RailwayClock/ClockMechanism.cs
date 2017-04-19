using System;
using System.Windows.Threading;

namespace RailwayClock
{
    class ClockMechanism
    {
        /*Position of clock hands. X and Y properties contains
        clock hands coordinates */
        /// <summary>
        /// Gets point with Cos and Sin to calculate X2 and Y2 coordinates of the clock seconds hand.
        /// </summary>
        private PointCoordinates SecondsHandPosition
        {
            get; set;
        }
        /// <summary>
        /// Gets point with Cos and Sin to calculate X2 and Y2 coordinates of the clock minutes hand.
        /// </summary>
        private PointCoordinates MinutesHandPosition
        {
            get; set;
        }
        /// <summary>
        /// Gets point with Cos and Sin to calculate X2 and Y2 coordinates of the clock hour hand.
        /// </summary>
        private PointCoordinates HoursHandPosition
        {
            get; set;
        }

        /*Moves the whole clock mechanism, works as a spring in clock.*/
        private DispatcherTimer clockspring = new DispatcherTimer();

        /*Delegates which are used to provide a junction for clock hands connected to ClockMechanism*/
        public Action<double, double> HourHandJunction;
        public Action<double, double> MinutesHandJunction;
        public Action<double, double> SecondsHandJunction;

        /*Gears of clock hands. Rotation of 360 degree of the secondsHandGear rotate 
        the minuteHandsGear on the next position and rotation of 360 degree of the minuteHandsGear
        rotate the hoursHandGear on the next position. Rotation is realised by the MoveClockGears method. */
        int secondsHandGear = 0;
        int minutesHandGear = 0;
        int hoursHandGear = 0;
        private int SecondsHandGear
        {
            get
            {
                return secondsHandGear;
            }

            set
            {
                secondsHandGear = value;
                SecondsHandPosition = setClockHandsPosition(SecondsHandGear);
                SecondsHandJunction?.Invoke(SecondsHandPosition.Cos, SecondsHandPosition.Sin);
            }
        }
        private int MinutesHandGear
        {
            get
            {
                return minutesHandGear;
            }

            set
            {
                minutesHandGear = value;
                MinutesHandPosition = setClockHandsPosition(minutesHandGear);
                MinutesHandJunction?.Invoke(MinutesHandPosition.Cos, MinutesHandPosition.Sin);
            }
        }
        private int HoursHandGear
        {
            get
            {
                return hoursHandGear;
            }

            set
            {
                hoursHandGear = value;
                HoursHandPosition = setClockHandsPosition(hoursHandGear);
                HourHandJunction?.Invoke(HoursHandPosition.Cos, HoursHandPosition.Sin);
            }
        }

        /// <summary>
        /// Constructor sets of the clock hands coordinates to current hour, minutes and seconds.
        /// </summary>
        public ClockMechanism() : this(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
        {

        }

        /// <summary>
        /// Constructor sets of the clock hands coordinates to hour, minutes and seconds taken from date parameter.
        /// </summary>
        /// <param name="date">A date with hour, minutes and seconds to sets coordinates.</param>
        public ClockMechanism(DateTime date) : this(date.Hour, date.Minute, date.Second)
        {

        }

        /// <summary>
        /// Constructor sets of the clock hands coordinates to hour, minutes and seconds taken from parameters.
        /// </summary>
        /// <param name="hour">Sets an hours of the mechanism</param>
        /// <param name="minutes">Sets minutes of the mechanism</param>
        /// <param name="seconds">Sets seconds of the mechanism</param>
        public ClockMechanism(int hour, int minutes, int seconds)
        {
            setGearsPosition(hour, minutes, seconds);
        }

        /// <summary>
        /// Runs the whole clock mechanism to the point of the next second.
        /// </summary>
        private void moveClockGears()
        {
            SecondsHandGear++;
            if (SecondsHandGear == 60)
            {
                SecondsHandGear = 0;
                MinutesHandGear++;

                if (MinutesHandGear == 60)
                {
                    MinutesHandGear = 0;
                    HoursHandGear++;
                }
                else if (MinutesHandGear % 12 == 0)
                {
                    HoursHandGear++;
                }
            }
        }

        /// <summary>
        /// Calculates Cos and Sin of the hands position. 
        /// </summary>
        /// <param name="gear">A gear of the chosen hand </param>
        /// <returns>Returns Point with calculated Cos and Sin properies </returns>
        private PointCoordinates setClockHandsPosition(int gear)
        {
            PointCoordinates gearPositionPoint = new PointCoordinates();
            gearPositionPoint.Cos = Math.Cos(((gear * 6) - 90) * (Math.PI / 180));
            gearPositionPoint.Sin = Math.Sin(((gear * 6) - 90) * (Math.PI / 180));
            return gearPositionPoint;
        }

        /// <summary>
        /// Sets gears position acording to parameters.
        /// </summary>
        /// <param name="hour">Sets an hours of the mechanism</param>
        /// <param name="minutes">Sets minutes of the mechanism</param>
        /// <param name="seconds">Sets seconds of the mechanism</param>
        private void setGearsPosition(int hour, int minutes, int seconds)
        {
            SecondsHandGear = (seconds >= 0 && seconds <= 59) ? seconds : SecondsHandGear;
            MinutesHandGear = (minutes >= 0 && minutes <= 59) ? minutes : MinutesHandGear;

            if (hour >= 0 && hour < 12)
            {
                HoursHandGear = (hour * 5) + (minutes / 12);
            }
            else if (hour >= 12 && hour <= 23)
            {
                HoursHandGear = ((hour - 12) * 5) + (minutes / 12);
            }
        }
        /// <summary>
        /// Sets a new hour, minutes and seconds in clock mechanism.
        /// </summary>
        /// <param name="newHour">A new hour to set. Put -1 if you want to keep an old value.</param>
        /// <param name="newMinutes">New minutes to set. Put -1 if you want to keep an old value.</param>
        /// <param name="newSeconds">New seconds to set. Put -1 if you want to keep an old value.</param>
        public void changeClockHandsPosition(int newHour, int newMinutes, int newSeconds)
        {
            /*If a newHour is in a proper range then assign a newHour to hour varialble, 
              if not then calculate an old hour from hoursHandGear position and assign.*/
            int hour = (newHour >= 0 && newHour <= 23) ? newHour : (HoursHandGear - (MinutesHandGear / 12)) / 5;
            setGearsPosition(hour, newMinutes, newSeconds);
        }
        /// <summary>
        /// Starts the clock mechanism.
        /// </summary>
        public void Start()
        {
            /*Invoking the HandJunction sets clock hands in proper position before the clock mechanism move first time.*/
            SecondsHandJunction?.Invoke(SecondsHandPosition.Cos, SecondsHandPosition.Sin);
            MinutesHandJunction?.Invoke(MinutesHandPosition.Cos, MinutesHandPosition.Sin);
            HourHandJunction?.Invoke(HoursHandPosition.Cos, HoursHandPosition.Sin);
            clockspring.Interval = TimeSpan.FromMilliseconds(982); ;
            clockspring.Tick += Clockspring_Tick; ;
            clockspring.Start();
        }

        /// <summary>
        /// Runs the clock mechanism periodicaly, according to interval setting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clockspring_Tick(object sender, EventArgs e)
        {
            moveClockGears();
        }

        /// <summary>
        /// Stops the closk mechanism.
        /// </summary>
        public void Stop()
        {
            clockspring.Stop();
        }
    }

    /// <summary>
    /// Struct PointCoordinates contains a Cos and Sin properties which store 
    /// calculated values of sinus and cosinus of the gear position used 
    /// to calculate X and Y coordinates.
    /// </summary>
    struct PointCoordinates
    {
        public double Cos { get; set; }
        public double Sin { get; set; }
    }
}
