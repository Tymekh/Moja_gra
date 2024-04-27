using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Rectangle = System.Windows.Shapes.Rectangle;
using Point = System.Windows.Point;
using static monkeyTowerDefenceTD7.Pociski;
using System.Windows.Media.Imaging;
using Moja_gra;

namespace monkeyTowerDefenceTD7
{
    internal static class Pociski
    {
        private static DispatcherTimer BulletTimer = new DispatcherTimer();
        private static List<Bullets> BulletList = new List<Bullets>();
        private static Canvas canvas;
        public class Bullets // lista właściwości pocisków
        {
            public Rectangle Bullet { get; set; }
            public double Lifetime { get; set; }
            public double LifetimeLimit { get; set; }
            public int Dmg { get; set; }
            public double Angle { get; set; }
        }
        private static void BulletTimer_Tick(object? sender, EventArgs e) // timer dla pocisków
        {
            for (int i = 0; i < BulletList.Count; i++) // pętla dla wszyskich pocisków
            {
                Rectangle Bullet = BulletList[i].Bullet;
                BulletList[i].Lifetime += (double)1 / 60;
                int Damage = BulletList[i].Dmg;
                double Angle = BulletList[i].Angle;


                double BulletSpeed = 10; // szybkość pocisku

                double xMovement = Math.Cos(Angle) * BulletSpeed;
                double yMovement = Math.Sin(Angle) * BulletSpeed;

                Canvas.SetLeft(Bullet, Canvas.GetLeft(Bullet) + xMovement);
                Canvas.SetTop(Bullet, Canvas.GetTop(Bullet) + yMovement);


                //if (CheckColision(Bullet, Target)) // wykonuje się jeśli pocisk dotyka celu
                //{
                    ////MessageBox.Show("Trafiono");
                    //DeleteBullet(i);
                    //if (Target.CzyZyje) Target.ZadajObrazenia(Damage); // jeśli cel żyje zadaje obrażenia balonowi
                    //continue;
                //}

                if (BulletList[i].Lifetime > BulletList[i].LifetimeLimit) // Jeśli pocisk żyje dłużej niż określony czas usuwa pocisk
                {
                    DeleteBullet(i);
                }
            }
        }

        public static void Shot(Point StartPoint, double LifetimeLimit, double Size, int Damage, double Angle, int StartingDistance = 0) // "strzela" - towrzy pocisk i dodaje go do listy
        {
            Rectangle bullet = new Rectangle
            {
                Width = Size,
                Height = Size,
                Fill = Brushes.Black,
            };
            //BulletList.Add(bullet);
            //TargetList.Add(target);
            Panel.SetZIndex(bullet, 2);
            BulletList.Add(new Bullets // dodaje do listy właściwośći pocisku
            {
                Bullet = bullet,
                Lifetime = 0,
                LifetimeLimit = LifetimeLimit,
                Dmg = Damage,
                Angle = Angle,
            });

            Canvas.SetLeft(bullet, StartPoint.X - bullet.Width / 2);
            Canvas.SetTop(bullet, StartPoint.Y - bullet.Height / 2);

            double xMovement = Math.Cos(Angle) * StartingDistance;
            double yMovement = Math.Sin(Angle) * StartingDistance;

            Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) + xMovement);
            Canvas.SetTop(bullet, Canvas.GetTop(bullet) + yMovement);

            MainWindow.MyCanvas.Children.Add(bullet);

            if (BulletTimer.IsEnabled == false) // sprawdza czy timer jest włączony żeby uniknąć włączania go kilka razy
            {
                canvas = MainWindow.MyCanvas;
                BulletTimer = new DispatcherTimer();
                BulletTimer.Interval = TimeSpan.FromSeconds((double)1 / 60);
                BulletTimer.Tick += BulletTimer_Tick;
                BulletTimer.Start();
            }
        }

        private static void DeleteBullet(int id) // usuwa pocisk
        {
            canvas.Children.Remove(BulletList[id].Bullet);
            BulletList.RemoveAt(id);
        }

        private static bool CheckColision(FrameworkElement point1, FrameworkElement point2) // sprawdza czy 2 elementy się zderzają
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

        private static double CalculateAngle(FrameworkElement point1, FrameworkElement point2) // oblicza kąt pomiędzy elementami
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
