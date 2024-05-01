using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Moja_gra
{
    /// <summary>
    /// Logika interakcji dla klasy Gun.xaml
    /// </summary>
    public partial class Gun : UserControl
    {
        private DispatcherTimer GunTimer = new DispatcherTimer();
        double angle;
        Rectangle GunRectangle;
        Player Parent;
        private int id;
        private Random random = new Random();

        public Gun(Player parent, int id)
        {
            InitializeComponent();
            this.id = id;
            Parent = parent;
            GunTimerStart();
        }


        private void GunTimerStart()
        {
            GunTimer = new DispatcherTimer();
            GunTimer.Interval = TimeSpan.FromSeconds((double)1 / 60);
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
            double width = 50;
            double height = 50;
            switch (id)
            {
                case 0:
                    image.ImageSource = new BitmapImage(new Uri(@"pack://application:,,/img/gun.png"));
                    break;

                case 1:
                    image.ImageSource = new BitmapImage(new Uri(@"pack://application:,,/img/shotgun.png"));
                    width = 100;
                    break;

                default:
                    break;
            }
            // Create the rectangle
            Rectangle rectangle = new Rectangle
            {
                Width = width,
                Height = height,
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
            if (GunRectangle != null)
            {
                if (MainWindow.Player.ShotsRemaining >= 1)
                {
                    MainWindow.Player.ShotsRemaining -= 1;
                    MainWindow.Hud.RemoveBullet();
                    Point ParentPosition = new Point(Canvas.GetLeft(Parent) + Parent.ActualWidth / 2, Canvas.GetTop(Parent) + Parent.ActualHeight / 2);
                    Point MousePosition = new Point(MainWindow.Mouse_x, MainWindow.Mouse_y);

                    double angle = CalculateAngle(ParentPosition, MousePosition);

                    double GunRectangleCentre_X = Canvas.GetLeft(GunRectangle) + GunRectangle.Width / 2;
                    double GunRectangleCentre_Y = Canvas.GetTop(GunRectangle) + GunRectangle.Height / 2;

                    Point point = new Point(GunRectangleCentre_X, GunRectangleCentre_Y);

                    switch (id)
                    {
                        case 0:
                            Bullet.Shot(point, 20, 10, 10, angle);
                            return; 
                        case 1:
                            Bullet.Shot(point, 5, 10, 10, angle + RandomAngleVariaton());
                            Bullet.Shot(point, 5, 10, 10, angle + RandomAngleVariaton());
                            Bullet.Shot(point, 5, 10, 10, angle + RandomAngleVariaton());

                            double Knockback = 10;

                            double VxMovement = Math.Cos(angle) * Knockback;
                            double VyMovement = Math.Sin(angle) * Knockback;

                            Parent.Vx -= VxMovement;
                            Parent.Vy -= VyMovement;
                            return;
                    }
                }
            }
        }
        private double RandomAngleVariaton()
        {
            double AngleVariation = (random.NextDouble() - 0.5) / 2;
            return AngleVariation;
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
