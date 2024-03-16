using System.Numerics;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Threading;
using System.Threading;
using System.Timers;
using System.Printing.IndexedProperties;

namespace Moja_gra
{
    public class Bullet
    {
        private Canvas canvas;
        private DispatcherTimer bulletTimer = new DispatcherTimer();
        private Rectangle rectangle;
        double angle;
        double rectangleAngle;

        public Bullet(Canvas canvas)
        {
            this.canvas = canvas;
            bulletTimerStart();
        }

        private void bulletTimerStart()
        {
            bulletTimer = new DispatcherTimer();
            bulletTimer.Interval = TimeSpan.FromMilliseconds(1);
            bulletTimer.Tick += bulletTimer_Tick;
            bulletTimer.Start();
        }

        private void bulletTimer_Tick(object? sender, EventArgs e)
        {
            if (rectangle != null)
            {
                // Access the position of the rectangle here
                double currentX = Canvas.GetLeft(rectangle);
                double currentY = Canvas.GetTop(rectangle);

                // Example: Move the rectangle
                Canvas.SetLeft(rectangle, currentX + 10);
                Canvas.SetTop(rectangle, currentY + 10);
            }
        }

        public void CreateRectangle(double x, double y, double Mouse_x, double Mouse_y)
        {
            // Create the rectangle
            Rectangle rectangle = new Rectangle
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Black
            };

            // Set the position of the rectangle
            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);

            rectangleAngle = calculateAngle(x,y,Mouse_x,Mouse_y);
            // Add the rectangle to the canvas
            canvas.Children.Add(rectangle);
        }

        public double calculateAngle(double x1, double y1, double x2, double y2)
        {
            angle = Math.Atan2((y2 - y1), (x2 - x1)) * 180 / Math.PI;
            if (angle < 0) angle += 360;
            return angle;
        }
    }
}
