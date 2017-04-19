using System.Windows.Shapes;

namespace RailwayClock
{
    class ClockHandLine
    {
        Line hand = new Line();
        public Line Hand { get { return hand; } set { hand = value; } }
        int handLength = 0;
        public ClockHandLine(int lenght, int xJoinCoordinate, int yJoinCoordinate, SolidColorBrush clockHandColor, int clockHandThickness)
        {
            handLength = lenght;
            Hand.X1 = xJoinCoordinate;
            Hand.Y1 = yJoinCoordinate;
            hand.Stroke = clockHandColor;
            hand.StrokeThickness = clockHandThickness;
        }

        public void SetCoordinatesToClockHand(double cosCoordinate, double sinCoordinate)
        {
            Hand.X2 = handLength * cosCoordinate + Hand.X1;
            Hand.Y2 = handLength * sinCoordinate + Hand.Y1;
        }
    }
}
