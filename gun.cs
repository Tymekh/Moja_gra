using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Moja_gra
{
    public class Gun
    {
        private DispatcherTimer GunTimer = new DispatcherTimer();
        double angle;
        Rectangle GunRectangle;
        private Bullet bullet;
        FrameworkElement Parent;

        public Gun(FrameworkElement parent)
        {
            Parent = parent;
            GunTimerStart();
            bullet = new Bullet();
        }

        private void GunTimerStart()
        {
            GunTimer = new DispatcherTimer();
            GunTimer.Interval = TimeSpan.FromMilliseconds(1);
            GunTimer.Tick += GunTimer_Tick;
            GunTimer.Start();
        }

        private void GunTimer_Tick(object? sender, EventArgs e)
        {
            if (GunRectangle != null)
            {
                Point ParentPosition = new Point(Canvas.GetLeft(Parent) + Parent.ActualWidth / 2, Canvas.GetTop(Parent) + Parent.ActualHeight / 2);
                Point MousePosition = new Point(MainWindow.Mouse_x, MainWindow.Mouse_y);

                double angle = CalculateAngle(ParentPosition, MousePosition);


                int distance = 30;

                // change in movement
                double xMovement = Math.Cos(angle) * distance;
                double yMovement = Math.Sin(angle) * distance;

                Canvas.SetLeft(GunRectangle, Canvas.GetLeft(Parent) + Parent.ActualWidth / 2 - GunRectangle.ActualWidth / 2 + xMovement);
                Canvas.SetTop(GunRectangle, Canvas.GetTop(Parent) + Parent.ActualHeight / 2 - GunRectangle.ActualHeight / 2 + yMovement);

                //center of gun
                double GunRectangleCentre_X = Canvas.GetLeft(GunRectangle) + GunRectangle.Width / 2;
                double GunRectangleCentre_Y = Canvas.GetTop(GunRectangle) + GunRectangle.Height / 2;

                RotateTransform rotation = new RotateTransform(angle * 180 / Math.PI);
                GunRectangle.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                GunRectangle.RenderTransform = rotation;
            }
        }

        public void createGun()
        {
            ImageBrush image = new ImageBrush { };
            image.ImageSource = new BitmapImage(new Uri(@"pack://application:,,/img/gun.png"));
            // Create the rectangle
            Rectangle rectangle = new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = image
            };
            GunRectangle = rectangle;

            Canvas.SetLeft(rectangle, Canvas.GetLeft(Parent) + Parent.ActualWidth / 2);
            Canvas.SetTop(rectangle, Canvas.GetTop(Parent) + Parent.ActualHeight / 2);


            // Add the rectangle to the MainWindow.MyCanvas
            MainWindow.MyCanvas.Children.Add(rectangle);

        }

        public void Shot()
        {
            if(GunRectangle != null)
            {
                Point ParentPosition = new Point(Canvas.GetLeft(Parent) + Parent.ActualWidth / 2, Canvas.GetTop(Parent) + Parent.ActualHeight / 2);
                Point MousePosition = new Point(MainWindow.Mouse_x, MainWindow.Mouse_y);

                double angle = CalculateAngle(ParentPosition, MousePosition);

                double GunRectangleCentre_X = Canvas.GetLeft(GunRectangle) + GunRectangle.Width / 2;
                double GunRectangleCentre_Y = Canvas.GetTop(GunRectangle) + GunRectangle.Height / 2;

                Point point = new Point(GunRectangleCentre_X, GunRectangleCentre_Y);
                bullet.createBullet(point,angle);
            }
        }
        private double CalculateAngle(Point point1, Point point2) // oblicza kąt pomiędzy elementami
        {
            if (point1 == null || point2 == null) { return 0; }

            double x1 = point1.X;
            double y1 = point1.Y;
            double x2 = point2.X;
            double y2 = point2.Y;

            double angle = Math.Atan2((y2 - y1), (x2 - x1));
            return angle;
        }
    }
}
