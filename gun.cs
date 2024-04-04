using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
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
        private Canvas canvas;
        private DispatcherTimer GunTimer = new DispatcherTimer();
        double angle;
        Rectangle GunRectangle;

        public Gun(Canvas canvas)
        {
            this.canvas = canvas;
            GunTimerStart();
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
                double angle = calculateAngle(MainWindow.Player_x, MainWindow.Player_y, MainWindow.Mouse_x, MainWindow.Mouse_y);
                //double angle = -1.5707;

                RotateTransform rotation = new RotateTransform(angle * 180 / Math.PI);
                //rotation.CenterX = GunRectangle.Width / 2;
                //rotation.CenterY = GunRectangle.Height / 2;
                GunRectangle.RenderTransformOrigin = new System.Windows.Point(0, 0);
                GunRectangle.RenderTransform = rotation;

                // change in movement
                double xMovement = Math.Cos(angle) * 30;
                double yMovement = Math.Sin(angle) * 30;

                Canvas.SetLeft(GunRectangle, MainWindow.Player_x + xMovement);
                Canvas.SetTop(GunRectangle, MainWindow.Player_y + yMovement);

                //center of gun
                double GunRectangleCentre_X = Canvas.GetLeft(GunRectangle) + GunRectangle.Width / 2;
                double GunRectangleCentre_Y = Canvas.GetTop(GunRectangle) + GunRectangle.Height / 2;

                //Canvas.SetLeft(GunRectangle, GunRectangleCentre_X);
                //Canvas.SetTop(GunRectangle, GunRectangleCentre_Y);
            }
        }

        public void createGun()
        {
            ImageBrush image = new ImageBrush { };
            image.ImageSource = new BitmapImage(new Uri(@"pack://application:,,/img/gun.png"));
            // Create the rectangle
            Rectangle rectangle = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = image
            };
            GunRectangle = rectangle;

            Canvas.SetLeft(rectangle, MainWindow.Player_x);
            Canvas.SetTop(rectangle, MainWindow.Player_y);


            // Add the rectangle to the canvas
            canvas.Children.Add(rectangle);

        }
        public double calculateAngle(double x1, double y1, double x2, double y2)
        {
            double angle;
            angle = Math.Atan2((y2 - y1), (x2 - x1)); //calculate angle in radians
            return angle;
        }
    }
}
