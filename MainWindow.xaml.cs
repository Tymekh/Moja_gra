﻿using System;
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
        public static Rectangle Podloga;

        public Bullet bullet { get; }
        public Gun Gun { get; }
        
        public MainWindow()
        {
            InitializeComponent();
            MyCanvas = MyGame;

            Podloga = Floor;
            DispatcherTimer gameTimer = new DispatcherTimer();
            gameTimer.Tick += gameTimer_tick;
            gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            gameTimer.Start();
            MyGame.Focus();
            //bullet = new Bullet(MyGame);
            Player gracz = new Player();
            gracz.createPlayer(100, 100);
            Player = gracz;
            Gun = new Gun();
            Gun.createGun(Player);
            MessageBox.Show("tak");
        }

        private void MyGame_LeftClick(object sender, MouseButtonEventArgs e)
        {
            Linia linia = new Linia();
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
            tekst.Text = stats;
            return angle;
        }

        private void gameTimer_tick(object? sender, EventArgs e)
        {
            //Updating public static variables
            //Player_x = Canvas.GetLeft(Player) + Player.ActualWidth / 2;
            //Player_y = Canvas.GetTop(Player) + Player.ActualHeight / 2; 
            Mouse_x = Mouse.GetPosition(Application.Current.MainWindow).X;
            Mouse_y = Mouse.GetPosition(Application.Current.MainWindow).Y;
            CalculateAngle();

            //Makes player apear on top
            //Canvas.SetZIndex(Player.player, 1);
        }


        private void KeyDown(object sender, KeyEventArgs e)
        {
            Player.PlayerMovement(sender, e);
            if (e.Key == Key.Q)
            {
                ;
            }
            if (e.Key == Key.P)
            {
                ;
            }
        }
    }
}

