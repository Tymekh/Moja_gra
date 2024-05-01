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
        public BulletCounter()
        {
            InitializeComponent();
            MainWindow.MyCanvas.Children.Add(this);
        }
        public void AddBullet(int ?Count = 1)
        {
            while (Images.Count < Count)
            {
                Image Bullet = new Image();
                Hud.Children.Add(Bullet);
                Images.Add(Bullet);
            }
        }

        public void RemoveBullet()
        {
            if(Images.Count > 0)
            {
                Hud.Children.Remove(Images[Images.Count - 1]);
                Images.RemoveAt(Images.Count - 1);
            }
        }
    }
}
