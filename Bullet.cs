using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Moja_gra
{
    public class Bullet
    {
        private DispatcherTimer bulletTimer = new DispatcherTimer();
        private Rectangle projectile1;
        //double angle;
        public static List<Rectangle> bulletList = new List<Rectangle>();
        List<double> bulletAngleList = new List<double>();
        private int BulletSpeed = 7;

        public Bullet()
        {
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
            if (projectile1 != null)
            {
                for (int i = 0; i < bulletList.Count; i++)
                {
                    Rectangle projectile = bulletList[i];
                    double angle = bulletAngleList[i];
                    if (isOutsideCanvas(projectile))
                    {
                        removeRectangle(projectile, i);
                    };

                    double xMovement = Math.Cos(angle) * BulletSpeed;
                    double yMovement = Math.Sin(angle) * BulletSpeed;

                    Canvas.SetLeft(projectile, Canvas.GetLeft(projectile) + xMovement);
                    Canvas.SetTop(projectile, Canvas.GetTop(projectile) + yMovement);
                }
            }
        }

        public void createBullet(Point point,double angle)
        {
            Rectangle projectile = new Rectangle
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Black
            };
            projectile1 = projectile;

            //double angle = calculateAngle(Player.Player_x, Player.Player_y, MainWindow.Mouse_x, MainWindow.Mouse_y);
            bulletList.Add(projectile);
            bulletAngleList.Add(angle);

            double distance = 25;

            double xMovement = Math.Cos(angle) * distance;
            double yMovement = Math.Sin(angle) * distance;

            double x = point.X - projectile.Width / 2;
            double y = point.Y - projectile.Height / 2;

            Canvas.SetLeft(projectile, x + xMovement);
            Canvas.SetTop(projectile, y + yMovement);

            MainWindow.MyCanvas.Children.Add(projectile);
        }

        public bool isOutsideCanvas(Rectangle projectile)
        {
            if (Canvas.GetLeft(projectile) < 0) 
            {
                return true;
            }
            if(Canvas.GetLeft(projectile) > MainWindow.MyCanvas.Width) 
            {
                return true;
            }
            if(Canvas.GetTop(projectile) < 0)
            {
                return true;
            }
            if(Canvas.GetTop(projectile) > MainWindow.MyCanvas.Height)
            {
                return true;
            }
            return false;
        }
        public void removeRectangle(Rectangle projectile, int index) 
        {
            MainWindow.MyCanvas.Children.Remove(projectile);
            bulletList.RemoveAt(index);
            bulletAngleList.RemoveAt(index);
        }
        public double calculateAngle(double x1, double y1, double x2, double y2)
        {
            double angle;
            angle = Math.Atan2((y2 - y1), (x2 - x1)); //calculate angle in radians
            return angle;
        }
    }
}
