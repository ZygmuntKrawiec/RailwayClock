using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RailwayClock
{
    class ClockFace
    {
        Canvas background;
        /// <summary>
        /// Returns clock face with seconds markers.
        /// </summary>
        public Canvas Background { get { return background; } }
        public ClockFace()
        {
            background = new Canvas();
            background.Children.Add(backgroundOfFaceClock);
            setSecondsMarkers();
        }

        /*Represents a base of the clock face*/
        Ellipse backgroundOfFaceClock = new Ellipse()
        {
            Name = "faceOfClock",
            Height = 300,
            Width = 300,
            Fill = Brushes.Beige,
        };

        /// <summary>
        /// Creates markers to show seconds and minutes positions on a clock face.
        /// Makes a marker thicker when it also indicates an hour.
        /// </summary>
        private void setSecondsMarkers()
        {
            for (int i = 0; i < 60; i++)
            {
                int pointerLenght = 140;
                Line pointerLine = new Line()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                };
                if (i % 5 == 0)
                {
                    pointerLine.StrokeThickness *= 5;
                }

                if (new[] { 0, 15, 30, 45 }.Contains(i))
                {
                    pointerLenght = 125;
                }
                else pointerLenght = 135;
                pointerLine.X1 = pointerLenght * Math.Cos(((i * 6) - 90) * (Math.PI / 180)) + 150;
                pointerLine.Y1 = pointerLenght * Math.Sin(((i * 6) - 90) * (Math.PI / 180)) + 150;

                pointerLine.X2 = 145 * Math.Cos(((i * 6) - 90) * (Math.PI / 180)) + 150;
                pointerLine.Y2 = 145 * Math.Sin(((i * 6) - 90) * (Math.PI / 180)) + 150;

                background.Children.Add(pointerLine);
            }
        }
    }
}
