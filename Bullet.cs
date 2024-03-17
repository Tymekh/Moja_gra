using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Moja_gra
{
    public class Bullet
    {
        private Canvas canvas;
        private DispatcherTimer bulletTimer = new DispatcherTimer();
        private Rectangle rectangle1;
        double angle;
        public static double rectangleAngle;
        public static List<Rectangle> bulletList = new List<Rectangle>();
        List<double> bulletAngleList = new List<double>();

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
            if (rectangle1 != null)
            {
                for (int i = 0; i < bulletList.Count; i++)
                {
                    Rectangle rectangle = bulletList[i];
                    double angle = bulletAngleList[i];
                    isOutsideCanvas(rectangle, i);

                    double xMovement = Math.Cos(angle) * 5;
                    double yMovement = Math.Sin(angle) * 5;

                    Canvas.SetLeft(rectangle, Canvas.GetLeft(rectangle) + xMovement);
                    Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) + yMovement);
                }
            }
        }

        public void createRectangle(double x, double y, double Mouse_x, double Mouse_y)
        {
            // Create the rectangle
            Rectangle rectangle = new Rectangle
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Black
            };
            rectangle1 = rectangle;

            //Set the position of the rectangle
            Canvas.SetLeft(rectangle, MainWindow.Player_x);
            Canvas.SetTop(rectangle, MainWindow.Player_y);

            double Angle = calculateAngle(MainWindow.Player_x, MainWindow.Player_y, MainWindow.Mouse_x, MainWindow.Mouse_y);
            //Adding rectangles to list to track all of them
            bulletList.Add(rectangle);
            bulletAngleList.Add(angle);

            // Add the rectangle to the canvas
            canvas.Children.Add(rectangle);

        }

        public void isOutsideCanvas(Rectangle rectangle, int index)
        {
            if (Canvas.GetLeft(rectangle) < 0) 
            {
                removeRectangle(rectangle, index);
            }
            if(Canvas.GetLeft(rectangle) > canvas.Width) 
            {
                removeRectangle(rectangle, index);
            }
            if(Canvas.GetTop(rectangle) < 0)
            {
                removeRectangle(rectangle, index);
            }
            if(Canvas.GetTop(rectangle) > canvas.Height)
            {
                removeRectangle(rectangle, index);
            }
        }
        public void removeRectangle(Rectangle rectangle, int index) 
        {
            canvas.Children.Remove(rectangle);
            bulletList.RemoveAt(index);
            bulletAngleList.RemoveAt(index);
        }
        public double calculateAngle(double x1, double y1, double x2, double y2)
        {
            angle = Math.Atan2((y2 - y1), (x2 - x1)); //calculate angle in radians
            return angle;
        }
    }
}
