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

namespace Moja_gra
{
    /// <summary>
    /// Logika interakcji dla klasy BulletCounter.xaml
    /// </summary>
    public partial class BulletCounter : UserControl
    {
        public static int BulletCount;
        private static List<Image> Images = new List<Image>();
        private int DepletedBulletsCount = 0;
        BitmapImage NormalBulletSource = new BitmapImage(new Uri(@"pack://application:,,/img/Bullet.png"));
        BitmapImage DepletedBulletSource = new BitmapImage(new Uri(@"pack://application:,,/img/BulletDepleted.png"));
        public BulletCounter()
        {
            InitializeComponent();
            AddEmptyBullets();
            MainWindow.MyCanvas.Children.Add(this);
        }
        public void AddEmptyBullets()
        {
            int Count = BulletCount;
            while (DepletedBulletsCount < MainWindow.Player.MagSize)
            {
                Image Bullet = new Image();
                Bullet.Source = DepletedBulletSource;
                DepletedHud.Children.Add(Bullet);
                Images.Add(Bullet);
                DepletedBulletsCount++;
            }
        }
        public void AddBullet(int ?Count = 1)
        {
            RemoveBullet(Images.Count);
            while (Images.Count < Count)
            {
                Image Bullet = new Image();
                Bullet.Source = NormalBulletSource;
                Hud.Children.Add(Bullet);
                Images.Add(Bullet);
                //AddEmptyBullets();
            }
        }

        public void RemoveBullet(int ?Count = 1)
        {
            if(Images.Count > 0)
            {
                while(Count > 0)
                {
                    Hud.Children.Remove(Images[Images.Count - 1]);
                    Images.RemoveAt(Images.Count - 1);
                    Count--;
                }
            }
        }
    }
}
