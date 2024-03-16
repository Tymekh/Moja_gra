using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace Moja_gra
{
    internal class Linia
    {
        Canvas canvas;
        Shape line;
        private System.Drawing.Rectangle rect;
        public void draw_Linia(Canvas MyGame,int player_x, int player_y, int mouse_x, int mouse_y)
        {
            canvas = MyGame;

            Line line = new Line();
            //Thickness thickness = new Thickness(101, -11, 362, 250);
            //line.Margin = thickness;
            line.Visibility = System.Windows.Visibility.Visible;
            line.StrokeThickness = 4;
            line.Stroke = System.Windows.Media.Brushes.Black;
            line.X1 = player_x;
            line.Y1 = player_y;
            line.X2 = mouse_x;
            line.Y2 = mouse_y;
            MyGame.Children.Add(line);

            rect = new System.Drawing.Rectangle()
            {
                Height = 5,
                Width = 5
            };
            //MyGame.Children.Add(rect);
            //rect = new Rectangle
            //{
            //    Stroke = Brushes.LightBlue,
            //    StrokeThickness = 2
            //};
            //Canvas.SetLeft(rect, 10);
            //Canvas.SetTop(rect, 10);
            //canvas.Children.Add(rect)

            //canvasWrapPanel.Children.Add(new Rectangle
            //{
            //    Width = 50,
            //    Height = 50,
            //    StrokeThickness = 2,
            //    Name = "box" + i.ToString(),
            //    Stroke = new SolidColorBrush(Colors.Black),
            //    Margin = new Thickness(15)
            //});
        }
    }
}
