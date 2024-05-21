using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Moja_gra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Canvas MyCanvas;
        public static Player Player;
        public static BulletCounter Hud;
        public static double Mouse_x, Mouse_y;
        public static List<Rectangle> Obstacles = new List<Rectangle>();
        public static bool IsTouching;
        private static double HigestVy;
        public static Rectangle Coin;

        //public Bullet bullet { get; }
        public Gun Gun { get; }

        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += OnWindowSizeChanged;
            MyCanvas = MyGame;

            foreach (Rectangle r in MyCanvas.Children.OfType<Rectangle>())
            {
                if((string)r.Tag == "obstacle")
                {
                    Obstacles.Add(r);
                }
            }
            DispatcherTimer gameTimer = new DispatcherTimer();
            gameTimer.Tick += gameTimer_tick;
            gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            gameTimer.Start();
            MyGame.Focus();
            //bullet = new Bullet(MyGame);
            Player gracz = new Player();
            gracz.createPlayer(520, 370);
            Player = gracz;
            Gun = new Gun(Player, 1);
            Gun.createGun();
            Log log = new Log();
            log.Show();
            Coin = coin;
            BulletCounter Hud = new BulletCounter();
            MainWindow.Hud = Hud;
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            WindowWidth = e.NewSize.Width;
            WindowHeight = e.NewSize.Height;
            Player.UpdateSize(e.NewSize);
        }

        private void MyGame_LeftClick(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(MyGame);
            Gun.Shot();
        }
        private void MyGame_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Rectangle rect = new Rectangle
            //{
            //    Width = 50,
            //    Height = 50,
            //    Fill = Brushes.Black,
            //};
            //Canvas.SetLeft(rect, e.GetPosition(MyGame).X);
            //Canvas.SetTop(rect, e.GetPosition(MyGame).Y);
            //Obstacles.Add(rect);
            //MyCanvas.Children.Add(rect);
            Player.MagSize += 1;
            Hud.AddEmptyBullets();
        }

        private double CalculateAngle()
        {
            string stats = "";
            double angle;
            double x1 = Canvas.GetLeft(Player);
            double y1 = Canvas.GetTop(Player);
            double x2 = Mouse_x;
            double y2 = Mouse_y;
            HigestVy = Player.Vy < HigestVy ? HigestVy = Player.Vy : HigestVy = HigestVy;
            angle = Math.Atan2((y2 - y1), (x2 - x1));
            double angleDegrees = angle * 180 / Math.PI;
            stats += "-----------------------------------------\n";
            stats += "Angle in rad: "+angle.ToString() + "\n";
            stats += "Angle: " + angleDegrees.ToString() + "\n";
            stats += "Bullet Count: " + Bullet.BulletList.Count.ToString() + "\n";
            stats += "Vx: " + Player.Vx.ToString() + "\n";
            stats += "Vy: " + Player.Vy.ToString() + "\n";
            stats += "W: " + Keyboard.IsKeyDown(Key.W) + "\n";
            stats += "A: " + Keyboard.IsKeyDown(Key.A) + "\n";
            stats += "S: " + Keyboard.IsKeyDown(Key.S) + "\n";
            stats += "D: " + Keyboard.IsKeyDown(Key.D) + "\n";
            stats += "IsTouching: " + IsTouching + "\n"; 
            stats += "Obstacles count: "+ Obstacles.Count() +"\n";
            stats += "IsOnGround: " + Player.IsOnGround + "\n";
            stats += "TouchingLeft: " + Player.TouchingLeft + "\n";
            stats += "TouchingRight: " + Player.TouchingRight + "\n";
            stats += "TouchingTop: " + Player.TouchingTop + "\n";
            stats += "TouchingDown: " + Player.TouchingDown + "\n";
            stats += "higest negative Vx: " + HigestVy.ToString() + "\n";
            Log.Stats = stats;
            return angle;
        }

        private void gameTimer_tick(object? sender, EventArgs e)
        {
            //Updating public static variables
            Mouse_x = Mouse.GetPosition(MyGame).X;
            Mouse_y = Mouse.GetPosition(MyGame).Y;
            CalculateAngle();

            //Makes player apear on top
            //Canvas.SetZIndex(Player.player, 1);
        }


        private void KeyDown(object sender, KeyEventArgs e)
        {
            //Player.PlayerMovement(sender, e);
            if (Keyboard.IsKeyDown(Key.Q))
            {
                ;
            }
        }
    }
}

