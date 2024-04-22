using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
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
        public double Vx, Vy = 0;
        private double Gravity = 0.2;
        private double PlayerSpeed = 1;
        private double JumpPower = 8;
        private double HorizontalMovementReduce = 0.2;
        public bool IsOnGround = false;
        public bool TouchingLeft, TouchingRight, TouchingDown, TouchingTop;
        private bool IsTouching = false;

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
            CheckAllColisions();
            if (IsTouching)
            {
                Vx = 0;
                Vy = 0;
                MainWindow.IsTouching = true;
            }

            //Canvas.SetLeft(this, Canvas.GetLeft(this) + Vx);
            //Canvas.SetTop(this, Canvas.GetTop(this) + Vy);
            foreach (Rectangle Obstacle in MainWindow.Obstacles)
            {
                Canvas.SetLeft(Obstacle, Canvas.GetLeft(Obstacle) - Vx);
                Canvas.SetTop(Obstacle, Canvas.GetTop(Obstacle) - Vy);
            }
            //IsOnGround = true;
            UpdateTouching();

            if (!IsOnGround && !TouchingDown)
            {
                if((TouchingLeft || TouchingRight) && Vy > 0)
                {
                    Vy += Gravity / 10;
                    if(Vy > 1) { Vy = 1; }
                }
                else
                {
                    Vy += Gravity;
                }
            }

            if (Vx < 0) Vx += HorizontalMovementReduce;
            if (Vx > 0) Vx -= HorizontalMovementReduce;
            if (-HorizontalMovementReduce < Vx && Vx < HorizontalMovementReduce) Vx = 0;

            Player_x = Canvas.GetLeft(this);
            Player_y = Canvas.GetTop(this);
        }

        public void UpdateCamera(Size size)
        {
            Point NewPlayerPosition = new Point(size.Width / 2, size.Height / 2);
            foreach (Rectangle Obstacle in MainWindow.Obstacles)
            {
                double xDiff = Canvas.GetLeft(this) - Canvas.GetLeft(Obstacle);
                double yDiff = Canvas.GetTop(this) - Canvas.GetTop(Obstacle);

                Canvas.SetLeft(Obstacle, NewPlayerPosition.X - xDiff);
                Canvas.SetTop(Obstacle, NewPlayerPosition.Y - yDiff);
            }
         
            Canvas.SetLeft(this, NewPlayerPosition.X);
            Canvas.SetTop(this, NewPlayerPosition.Y);
        }

        public void createPlayer(double x, double y)
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
            MainWindow.MyCanvas.Children.Add(this);
        }

        public void PlayerMovement(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.A) && !TouchingLeft) goLeft();
            if (Keyboard.IsKeyDown(Key.D) && !TouchingRight) goRight();
            if (Keyboard.IsKeyDown(Key.W) && TouchingDown) Jump();
            //if (e.Key == Key.S) Down();
        }
        private void goLeft()
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) - PlayerSpeed);
            //foreach (Rectangle Obstacle in MainWindow.Obstacles)
            //{
            //    Canvas.SetLeft(Obstacle, Canvas.GetLeft(Obstacle) + PlayerSpeed);
            //}
            Vx -= PlayerSpeed;
        }
        private void goRight()
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + PlayerSpeed);
            //foreach (Rectangle Obstacle in MainWindow.Obstacles)
            //{
            //    Canvas.SetLeft(Obstacle, Canvas.GetLeft(Obstacle) - PlayerSpeed);
            //}
            Vx += PlayerSpeed;
        }
        private void Jump()
        {
            if(IsOnGround && TouchingDown == true)
            {
                Canvas.SetTop(this, Canvas.GetTop(this) - Gravity - 1);
                //foreach (Rectangle Obstacle in MainWindow.Obstacles)
                //{
                //    Canvas.SetTop(Obstacle, Canvas.GetTop(Obstacle) + Gravity);
                //}
                Vy -= JumpPower;
                //IsOnGround = false;
            }
        }

        public static bool CheckColision(FrameworkElement point1, FrameworkElement point2) // check if 2 elements are coliding
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
        public static bool CheckTouching(FrameworkElement point1, FrameworkElement point2) // check if 2 elements are touching
        {
            if (point1 == null || point2 == null) { return false; }

            var x1 = Canvas.GetLeft(point1);
            var y1 = Canvas.GetTop(point1);

            Rect HitBox1 = new Rect(x1, y1, point1.ActualWidth + 1, point1.ActualHeight + 1);

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

        private void UpdateTouching()
        {
            foreach (Rectangle Obstacle in MainWindow.Obstacles)
            {
                if (CheckTouching(down, Obstacle))
                {
                    TouchingDown = true;
                }
                if (CheckTouching(top, Obstacle))
                {
                    TouchingTop = true;
                }
                if (CheckTouching(left, Obstacle))
                {
                    TouchingLeft = true;
                }
                if (CheckTouching(right, Obstacle))
                {
                    TouchingRight = true;
                }
            }
            if(TouchingDown || TouchingTop || TouchingRight || TouchingLeft)
            {
                IsTouching = true;
            }
        }

        private static Point CalculateCenter(FrameworkElement point)
        {
            double x = Canvas.GetLeft(point) + point.ActualWidth / 2;
            double y = Canvas.GetTop(point) + point.ActualHeight / 2;
            return new Point(x, y);
        }

        private void CheckAllColisions()
        {
            TouchingLeft = false;
            TouchingRight = false;
            TouchingDown = false;
            TouchingTop = false;
            IsTouching = false;
            IsOnGround = false;
            foreach (Rectangle Obstacle in MainWindow.Obstacles)
            {
                if (CheckColision(Obstacle, this))
                {
                    // Most of this stuff would probably be good to keep stored inside the player
                    // along side their x and y position. That way it doesn't have to be recalculated
                    // every collision check
                    var playerHalfW = this.ActualWidth / 2;
                    var playerHalfH = this.ActualHeight / 2;
                    var playerCenterX = Canvas.GetLeft(this) + this.ActualWidth / 2;
                    var playerCenterY = Canvas.GetTop(this) + this.ActualHeight / 2;
                    var enemyHalfW = Obstacle.ActualWidth / 2;
                    var enemyHalfH = Obstacle.ActualHeight / 2;
                    var enemyCenterX = Canvas.GetLeft(Obstacle) + Obstacle.ActualWidth / 2;
                    var enemyCenterY = Canvas.GetTop(Obstacle) + Obstacle.ActualHeight / 2;

                    // Calculate the distance between centers
                    var diffX = playerCenterX - enemyCenterX;
                    var diffY = playerCenterY - enemyCenterY;

                    // Calculate the minimum distance to separate along X and Y
                    var minXDist = playerHalfW + enemyHalfW;
                    var minYDist = playerHalfH + enemyHalfH;

                    // Calculate the depth of collision for both the X and Y axis
                    var depthX = diffX > 0 ? minXDist - diffX : -minXDist - diffX;
                    var depthY = diffY > 0 ? minYDist - diffY : -minYDist - diffY;

                    // Now that you have the depth, you can pick the smaller depth and move
                    // along that axis.
                    if (depthX != 0 && depthY != 0)
                    {
                        if (Math.Abs(depthX) < Math.Abs(depthY)) 
                        {
                            // Collision along the X axis. React accordingly
                            if (depthX > 0)
                            {
                                // Left side collision
                                //MessageBox.Show("Lewo");
                                //foreach (Rectangle obstacle in MainWindow.Obstacles)
                                //{
                                //    Canvas.SetLeft(obstacle, Canvas.GetLeft(obstacle) - 3);
                                //}

                                Canvas.SetLeft(this, Canvas.GetLeft(Obstacle) + this.ActualWidth - 3);
                                Vx = 0;
                                TouchingLeft = true;
                            }
                            if (depthX < 0)
                            {
                                // Right side collision
                                //MessageBox.Show("Prawo");
                                Canvas.SetLeft(this, Canvas.GetLeft(Obstacle) - this.ActualWidth + 1);
                                Vx = 0;
                                TouchingRight = true;
                            }
                        }
                        // Collision along the Y axis.
                        else
                        {
                            if (depthY > 0)
                            {
                                // Top side collision
                                //MessageBox.Show("gora");
                                Canvas.SetTop(this, Canvas.GetTop(Obstacle) + Obstacle.ActualHeight);
                                Vy = 0;
                                TouchingTop = true;
                            }
                            if (depthY < 0)
                            {
                                // Bottom side collision
                                //MessageBox.Show("dol");
                                Canvas.SetTop(this, Canvas.GetTop(Obstacle) - this.ActualHeight + Gravity + 1);

                                ////double difference = Canvas.GetTop(this) + this.ActualHeight / 2 - Canvas.GetTop(obstacle);
                                //foreach (Rectangle obstacle in MainWindow.Obstacles)
                                //{
                                //    Canvas.SetTop(obstacle, Canvas.GetTop(obstacle) - Vy);
                                //}

                                Vy = 0;
                                IsOnGround = true;
                                TouchingDown = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
