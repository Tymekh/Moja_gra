using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
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
        public bool IsTouching = false;
        private double UpdatedPlayerSpeed;
        private Rectangle NextXPositionRectangle;
        private Rectangle NextYPositionRectangle;
        private Brush NextXPositionColor = Brushes.Transparent;
        private Brush NextYPositionColor = Brushes.Transparent;
        private Size NewestSize;
        public  int ShotsRemaining = 0;
        public int MagSize = 2;

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
            ResetTouching();
            NextXPosition();
            NextYPosition();
            UpdateTouching();
            UpdatedPlayerSpeed = Math.Pow((PlayerSpeed/10)-0.5, 2) + 1;
            //CheckAllColisions();

            //Canvas.SetLeft(this, Canvas.GetLeft(this) + Vx);
            //Canvas.SetTop(this, Canvas.GetTop(this) + Vy);
            foreach (Rectangle Obstacle in MainWindow.Obstacles)
            {
                Canvas.SetLeft(Obstacle, Canvas.GetLeft(Obstacle) - Vx);
                Canvas.SetTop(Obstacle, Canvas.GetTop(Obstacle) - Vy);
            }
            //UpdateTouching();

            //gravity();

            double UpdatedHorizontalMovementReduce = HorizontalMovementReduce + Math.Pow(Vx,2) / 1000;
            if (Vx < 0) Vx += UpdatedHorizontalMovementReduce;
            if (Vx > 0) Vx -= UpdatedHorizontalMovementReduce;
            if (-UpdatedHorizontalMovementReduce < Vx && Vx < UpdatedHorizontalMovementReduce) Vx = 0;

            if (Vy > 20)
            {
                Vy -= Gravity;
            }
            if(Vy < -20)
            {
                Vy += Gravity;
            }
            //Vy = Vy < -20 || Vy > 20 ? Vy -= Gravity : Vy += Gravity;

            Player_x = Canvas.GetLeft(this);
            Player_y = Canvas.GetTop(this);
            UpdateCamera();
        }

        private void gravity()
        {
            if(IsOnGround || TouchingDown)
            {
                return;
            }
            if (!IsOnGround && !TouchingDown)
            {
                if ((TouchingLeft || TouchingRight) && Vy > 0)
                {
                    Vy += Math.Sqrt(Gravity) / 100;
                    //if(Vy > 1) { Vy = 1; }
                }
                else
                {
                    Vy += Gravity;
                }
            }
        }

        private void UpdateCamera()
        {
            Point NewPlayerPosition = new Point(NewestSize.Width / 2, NewestSize.Height / 2);
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
        public void UpdateSize(Size size)
        {
            NewestSize = size;
        }

        public void createPlayer(double x, double y)
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
            if (!TouchingLeft)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) - PlayerSpeed);
                //Vx -= PlayerSpeed;
                Vx -= UpdatedPlayerSpeed;
            }
        }
        private void goRight()
        {
            if(!TouchingRight)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + PlayerSpeed);
                //Vx += PlayerSpeed;
                Vx += UpdatedPlayerSpeed;
            }
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
            //if (TouchingLeft)
            //{
            //    Canvas.SetLeft(this, Canvas.GetLeft(this) + 1);
            //    Vy -= JumpPower;
            //    Vx += 5;
            //}
            //if (TouchingRight)
            //{
            //    Canvas.SetLeft(this, Canvas.GetLeft(this) - 1);
            //    Vy -= JumpPower;
            //    Vx -= 5;
            //}
        }

        private static bool CheckColision(FrameworkElement point1, FrameworkElement point2) // check if 2 elements are coliding
        {
            if (point1 == null || point2 == null) { return false; }

            var x1 = Canvas.GetLeft(point1);
            var y1 = Canvas.GetTop(point1);

            Rect HitBox1 = new Rect(x1, y1, point1.ActualWidth, point1.ActualHeight);

            var x2 = Canvas.GetLeft(point2);
            var y2 = Canvas.GetTop(point2);
            Rect HitBox2 = new Rect(x2, y2, point2.Width, point2.Height);

            if (HitBox1.IntersectsWith(HitBox2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckLeftColision(FrameworkElement point2) // check if 2 elements are coliding
        {
            var x1 = Canvas.GetLeft(this) - 1;
            var y1 = Canvas.GetTop(this) - 1;

            Rect HitBox1 = new Rect(x1, y1, this.ActualWidth - this.ActualWidth / 2, this.ActualHeight - 1);

            var x2 = Canvas.GetLeft(point2);
            var y2 = Canvas.GetTop(point2);
            Rect HitBox2 = new Rect(x2, y2, point2.Width, point2.Height);

            if (HitBox1.IntersectsWith(HitBox2))
            {
                TouchingLeft = true;
            }
            else
            {
                return;
            }
        }

        private void CheckRightColision(FrameworkElement point2) // check if 2 elements are coliding
        {
            var x1 = Canvas.GetLeft(this) + this.ActualWidth / 2;
            var y1 = Canvas.GetTop(this) - 1;

            Rect HitBox1 = new Rect(x1, y1, this.ActualWidth - this.ActualWidth / 2 + 1, this.ActualHeight - 1);

            var x2 = Canvas.GetLeft(point2);
            var y2 = Canvas.GetTop(point2);
            Rect HitBox2 = new Rect(x2, y2, point2.Width, point2.Height);

            if (HitBox1.IntersectsWith(HitBox2))
            {
                TouchingRight = true;
            }
            else
            {
                return;
            }
        }

        private void NextXPosition()
        {
            MainWindow.MyCanvas.Children.Remove(NextXPositionRectangle);
            if (Vx > 0)
            {
                Rectangle rect = new Rectangle
                {
                    Width = this.ActualWidth / 2+ Vx,
                    Height = this.ActualHeight - 1,
                    Fill = NextXPositionColor
                };
                Canvas.SetLeft(rect, Canvas.GetLeft(this) + this.ActualWidth / 2);
                Canvas.SetTop(rect, Canvas.GetTop(this));
                MainWindow.MyCanvas.Children.Add(rect);
                NextXPositionRectangle = rect;
            }
            if(Vx < 0)
            {
                Rectangle rect = new Rectangle
                {
                    Width = this.ActualWidth / 2 - Vx,
                    Height = this.ActualHeight - 1,
                    Fill = NextXPositionColor
                };
                Canvas.SetLeft(rect, Canvas.GetLeft(this) + Vx);
                Canvas.SetTop(rect, Canvas.GetTop(this));
                MainWindow.MyCanvas.Children.Add(rect);
                NextXPositionRectangle = rect;
            }
        }
        
        private void NextYPosition()
        {
            MainWindow.MyCanvas.Children.Remove(NextYPositionRectangle);
            // na dół
            if(Vy > 0)
            {
                Rectangle rct = new Rectangle
                {
                    Width = this.ActualWidth - 2,
                    Height = this.ActualHeight / 2 + Vy,
                    Fill = NextYPositionColor
                };
                Canvas.SetLeft(rct, Canvas.GetLeft(this) + 1);
                Canvas.SetTop(rct, Canvas.GetTop(this) + this.ActualHeight / 2);
                MainWindow.MyCanvas.Children.Add(rct);
                NextYPositionRectangle = rct;
            }
            // do góry
            if(Vy < 0)
            {
                Rectangle rect = new Rectangle
                {
                    Width = this.ActualWidth - 2,
                    Height = this.ActualHeight /2 - Vy,
                    Fill = NextYPositionColor
                };
                Canvas.SetLeft(rect, Canvas.GetLeft(this) + 1);
                Canvas.SetTop(rect, Canvas.GetTop(this) + Vy);
                MainWindow.MyCanvas.Children.Add(rect);
                NextYPositionRectangle = rect;
            }
        }
    
        private void ResetTouching()
        {
            TouchingLeft = false;
            TouchingRight = false;
            TouchingDown = false;
            TouchingTop = false;
            IsTouching = false;
            IsOnGround = false;
        }
        private void UpdateTouching()
        {
            gravity();
            foreach (Rectangle Obstacle in MainWindow.Obstacles)
            {
                if(CheckColision(this, MainWindow.Coin))
                {
                    MessageBox.Show("wygrałeś");
                    Environment.Exit(0);
                }
                CheckLeftColision(Obstacle);
                CheckRightColision(Obstacle);
                if(CheckColision(Obstacle, NextYPositionRectangle))
                {
                    // bottom
                    if(Vy > 0 && Vy != Gravity)
                    {
                        Canvas.SetTop(this, Canvas.GetTop(Obstacle) - this.ActualHeight);
                        Vy = 0;
                        MainWindow.MyCanvas.Children.Remove(NextYPositionRectangle);
                        NextYPositionRectangle = null;
                        MainWindow.MyCanvas.Children.Remove(NextXPositionRectangle);
                        NextXPositionRectangle = null;
                        IsOnGround = true;
                        TouchingDown = true;
                        ShotsRemaining = MagSize;
                        MainWindow.Hud.AddBullet(MagSize);
                    }else
                    // top
                    if (Vy < 0)
                    {
                        Canvas.SetTop(this, Canvas.GetTop(Obstacle) + Obstacle.ActualHeight + 0.1);
                        Vy = 0;
                        MainWindow.MyCanvas.Children.Remove(NextYPositionRectangle);
                        NextYPositionRectangle = null;
                        MainWindow.MyCanvas.Children.Remove(NextXPositionRectangle);
                        NextXPositionRectangle = null;
                        TouchingTop = true;
                    }
                }
                if(CheckColision(Obstacle, NextXPositionRectangle))
                {
                    // left
                    if(Vx < 0)
                    {
                        Canvas.SetLeft(this, Canvas.GetLeft(Obstacle) + Obstacle.ActualWidth);
                        TouchingLeft = true;
                        Vx = 0;
                        MainWindow.MyCanvas.Children.Remove(NextXPositionRectangle);
                        NextXPositionRectangle = null;
                    }
                    // right
                    if(Vx > 0)
                    {
                        Canvas.SetLeft(this, Canvas.GetLeft(Obstacle) - this.ActualWidth);
                        TouchingRight = true;
                        Vx = 0;
                        MainWindow.MyCanvas.Children.Remove(NextXPositionRectangle);
                        NextXPositionRectangle = null;
                    }
                }
            }
        }
    }
}
