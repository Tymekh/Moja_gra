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

        public Gun()
        {
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

                Point Mouse = new Point(MainWindow.Mouse_x, MainWindow.Mouse_y);

                Rectangle Mouse2 = new Rectangle { };
                Canvas.SetLeft(Mouse2, Mouse.X);
                Canvas.SetTop(Mouse2, Mouse.Y);

                double angle = CalculateAngle(Parent, Mouse2);

                RotateTransform rotation = new RotateTransform(angle * 180 / Math.PI);
                //rotation.CenterX = GunRectangle.Width / 2;
                //rotation.CenterY = GunRectangle.Height / 2;
                GunRectangle.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                GunRectangle.RenderTransform = rotation;
                int distance = 30;

                // change in movement
                double xMovement = Math.Cos(angle) * distance;
                double yMovement = Math.Sin(angle) * distance;

                Canvas.SetLeft(GunRectangle, Player.Player_x - GunRectangle.ActualWidth / 2 + xMovement);
                Canvas.SetTop(GunRectangle, Player.Player_y - GunRectangle.ActualHeight / 2 + yMovement);

                //center of gun
                double GunRectangleCentre_X = Canvas.GetLeft(GunRectangle) + GunRectangle.Width / 2;
                double GunRectangleCentre_Y = Canvas.GetTop(GunRectangle) + GunRectangle.Height / 2;

                //Canvas.SetLeft(GunRectangle, GunRectangleCentre_X);
                //Canvas.SetTop(GunRectangle, GunRectangleCentre_Y);
            }
        }

        public void createGun(FrameworkElement parent)
        {
            Parent = Parent;
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

            Canvas.SetLeft(rectangle, Player.Player_x);
            Canvas.SetTop(rectangle, Player.Player_y);


            // Add the rectangle to the MainWindow.MyCanvas
            MainWindow.MyCanvas.Children.Add(rectangle);

        }

        public void Shot()
        {
            if(GunRectangle != null)
            {
                Point Mouse = new Point(MainWindow.Mouse_x, MainWindow.Mouse_y);

                Rectangle Mouse2 = new Rectangle{};
                Canvas.SetLeft(Mouse2, Mouse.X);
                Canvas.SetTop(Mouse2, Mouse.Y);

                double angle = CalculateAngle(Parent, Mouse2);

                double GunRectangleCentre_X = Canvas.GetLeft(GunRectangle) + GunRectangle.Width / 2;
                double GunRectangleCentre_Y = Canvas.GetTop(GunRectangle) + GunRectangle.Height / 2;

                Point point = new Point(GunRectangleCentre_X, GunRectangleCentre_Y);
                bullet.createBullet(point,angle);
            }
        }
        private double CalculateAngle(FrameworkElement point1, FrameworkElement point2) // oblicza kąt pomiędzy elementami
        {
            if (point1 == null || point2 == null) { return 0; }

            double x1 = Canvas.GetLeft(point1) + point1.ActualWidth / 2;
            double y1 = Canvas.GetTop(point1) + point1.ActualHeight / 2;
            double x2 = Canvas.GetLeft(point2) + point2.ActualWidth / 2;
            double y2 = Canvas.GetTop(point2) + point2.ActualHeight / 2;

            double angle = Math.Atan2((y2 - y1), (x2 - x1));
            return angle;
        }
    }
}
