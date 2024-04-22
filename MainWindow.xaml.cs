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
        public static double Mouse_x, Mouse_y;
        public static List<Rectangle> Obstacles = new List<Rectangle>();
        public static Rectangle Podloga;
        public static bool IsTouching;
        private double HigestVy;

        public Bullet bullet { get; }
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
                Obstacles.Add(r);
            }
            Podloga = Floor;
            DispatcherTimer gameTimer = new DispatcherTimer();
            gameTimer.Tick += gameTimer_tick;
            gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            gameTimer.Start();
            MyGame.Focus();
            //bullet = new Bullet(MyGame);
            Player gracz = new Player();
            gracz.createPlayer(300, 200);
            Player = gracz;
            Gun = new Gun(Player);
            Gun.createGun();
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            WindowWidth = e.NewSize.Width;
            WindowHeight = e.NewSize.Height;
            Player.UpdateCamera(e.NewSize);
        }

        private void MyGame_LeftClick(object sender, MouseButtonEventArgs e)
        {
            //Linia linia = new Linia();
            //linia.draw_Linia(MyGame, (int)Canvas.GetLeft(Player) + 25, (int)Canvas.GetTop(Player) + 25, (int)Mouse.GetPosition(Application.Current.MainWindow).X, (int)Mouse.GetPosition(Application.Current.MainWindow).Y);
            //CalculateAngle();
            Point position = e.GetPosition(MyGame);
            //bullet.createRectangle(Canvas.GetLeft(Player)+25,Canvas.GetTop(Player)+25,position.X, position.Y
            Gun.Shot();

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
            stats += angle.ToString();
            stats += "\n";
            angle = angle * 180 / Math.PI;
            stats += angle.ToString();
            stats += "\n";
            stats += Bullet.bulletList.Count.ToString();
            stats += "\n";
            stats += "Vx: " + Player.Vx.ToString() + "\n";
            stats += "Vy: " + Player.Vy.ToString() + "\n";
            stats += "\n";
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
            tekst.Text = stats;
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
            Player.PlayerMovement(sender, e);
            if (Keyboard.IsKeyDown(Key.Q))
            {
                ;
            }
        }
    }
}

