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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Moja_gra
{
    /// <summary>
    /// Logika interakcji dla klasy Log.xaml
    /// </summary>
    public partial class Log : Window
    {
        DispatcherTimer UpdateTimer = new DispatcherTimer();
        public static string Stats;

        public Log()
        {
            InitializeComponent();
            UpdateTimer.Tick += UpdateValues;
            UpdateTimer.Interval = TimeSpan.FromSeconds((double)1/60);
            UpdateTimer.Start();
        }

        private void UpdateValues(object? sender, EventArgs e)
        {
            tekst.Text = Stats;
        }
    }
}
