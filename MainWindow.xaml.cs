using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer gameTimer = new DispatcherTimer();
            gameTimer.Tick += gameTimer_tick;
            gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            gameTimer.Start();
            MyGame.Focus();
            bullet = new Bullet(MyGame);
        }
        int PlayerSpeed = 5;
        bool keyLeft, keyRight, keyUp, keyDown;

        public Bullet bullet { get; }

        private void MyGame_LeftClick(object sender, MouseButtonEventArgs e)
        {
            Linia linia = new Linia();
            linia.draw_Linia(MyGame, (int)Canvas.GetLeft(Player) + 25, (int)Canvas.GetTop(Player) + 25, (int)Mouse.GetPosition(Application.Current.MainWindow).X, (int)Mouse.GetPosition(Application.Current.MainWindow).Y);
            //CalculateAngle();
            Point position = e.GetPosition(MyGame);
            bullet.CreateRectangle(Canvas.GetLeft(Player)+25,Canvas.GetTop(Player)+25,position.X, position.Y);

        }

        private void CalculateAngle()
        {
            double angle;
            int x1 = (int)Canvas.GetLeft(Player) + 25;
            int y1 = (int)Canvas.GetTop(Player) + 25;
            int x2 = (int)Mouse.GetPosition(Application.Current.MainWindow).X;
            int y2 = (int)Mouse.GetPosition(Application.Current.MainWindow).Y;
            angle = Math.Atan2((y2 - y1), (x2 - x1)) * 180 / Math.PI;
            //angle += 360;
            if(angle < 0) angle += 360;
            tekst.Text = angle.ToString();
        }

        private void gameTimer_tick(object? sender, EventArgs e)
        {
            if (keyLeft && Canvas.GetLeft(Player) > 0)
            {
                Canvas.SetLeft(Player, (Canvas.GetLeft(Player) - PlayerSpeed));
            }
            if (keyUp && Canvas.GetTop(Player) > 0)
            {
                Canvas.SetTop(Player, (Canvas.GetTop(Player) - PlayerSpeed));
            }
            if (keyRight && Canvas.GetLeft(Player) + Player.ActualWidth < 800)
            {
                Canvas.SetLeft(Player, (Canvas.GetLeft(Player) + PlayerSpeed));
            }
            if (keyDown && Canvas.GetTop(Player) + Player.ActualHeight < 450)
            {
                Canvas.SetTop(Player, (Canvas.GetTop(Player) + PlayerSpeed));
            }
            CalculateAngle();
            Canvas.SetZIndex(Player, 1);
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                keyLeft = true;
            }
            if (e.Key == Key.D)
            {
                keyRight = true;
            }
            if (e.Key == Key.W)
            {
                keyUp = true;
            }
            if (e.Key == Key.S)
            {
                keyDown = true;
            }
            if (e.Key == Key.Q)
            {
                ;
            }
            if (e.Key == Key.P)
            {
                ;
            }
        }
        private void KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                keyLeft = false;
            }
            if (e.Key == Key.D)
            {
                keyRight = false;
            }
            if (e.Key == Key.W)
            {
                keyUp = false;
            }
            if (e.Key == Key.S)
            {
                keyDown = false;
            }
        }
    }
}

