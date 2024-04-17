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
    /// Logika interakcji dla klasy Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        private static DispatcherTimer MovementTimer = new DispatcherTimer();
        public static double Player_x;
        public static double Player_y;
        public double Vx, Vy;
        private double Gravity = 0.1;
        private double PlayerSpeed = 0.5;
        private double JumpPower = 5;
        private double HorizontalMovementReduce = 0.2;
        private bool IsOnGround = false;

        public Player()
        {
            InitializeComponent();
            StartMovementTimer();
        }

        private void StartMovementTimer()
        {
            MovementTimer.Interval = TimeSpan.FromSeconds((double)1 / 60);
            MovementTimer.Tick += MovementTick;
            MovementTimer.Start();
        }

        private void MovementTick(object? sender, EventArgs e)
        {
            //if (!CheckColision(this, MainWindow.MyCanvas))
            //{
            //    ;
            //}
            Canvas.SetLeft(this, Canvas.GetLeft(this) + Vx);
            Canvas.SetTop(this, Canvas.GetTop(this) + Vy);
            if(CheckColision(this, MainWindow.Podloga))
            {
                if (Vy != Gravity)
                {
                    Canvas.SetTop(this, Canvas.GetTop(this) - Vy);
                }
                Vy = 0;
                IsOnGround = true;
            }
            else
            {
                Vy += Gravity;
            }

            if (Vx < 0) Vx += HorizontalMovementReduce;
            if (Vx > 0) Vx -= HorizontalMovementReduce;
            if (-HorizontalMovementReduce < Vx && Vx < HorizontalMovementReduce) Vx = 0;

            Player_x = Canvas.GetLeft(this);
            Player_y = Canvas.GetTop(this);
        }

        public void createPlayer(int x, int y)
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
            MainWindow.MyCanvas.Children.Add(this);
        }

        public void PlayerMovement(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.A)) goLeft();
            if (Keyboard.IsKeyDown(Key.D)) goRight();
            if (Keyboard.IsKeyDown(Key.W)) Jump();
            //if (e.Key == Key.S) Down();
        }
        private void goLeft()
        {
            Vx -= PlayerSpeed;
        }
        private void goRight()
        {
            Vx += PlayerSpeed;
        }
        private void Jump()
        {
            if(Vy == 0 && IsOnGround == true)
            {
                Vy -= JumpPower;
                IsOnGround = false;
            }
        }
        private void Down()
        {
            Vy += 5;
        }

        private static bool CheckColision(FrameworkElement point1, FrameworkElement point2) // check if 2 elements are coliding
        {
            if (point1 == null || point2 == null) { return false; }

            var x1 = Canvas.GetLeft(point1);
            var y1 = Canvas.GetTop(point1);

            Rect HitBox1 = new Rect(x1, y1, point1.ActualWidth, point1.ActualHeight);

            var x2 = Canvas.GetLeft(point2);
            var y2 = Canvas.GetTop(point2);
            Rect HitBox2 = new Rect(x2, y2, point2.ActualWidth, point2.ActualHeight);

            if (HitBox1.IntersectsWith(HitBox2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
