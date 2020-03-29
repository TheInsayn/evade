//using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Evade
{
    class Entity
    {
        //private Point pos = new Point();

        public Entity(int size, Color c) : this(size, c, 0, 0)
        {
        }

        public Entity(int size, Color c, int x, int y)
        {
            this.Size = size;
            this.X = x;
            this.Y = y;
            this.Color = c;
            this.Rect = new Rectangle()
            {
                Width = size,
                Height = size,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            Canvas.SetLeft(Rect, X);
            Canvas.SetTop(Rect, Y);
        }

        public int Size { get; set; }
        public int X { get; set; }

        public int Y { get; set; }

        public Color Color { get; set; }

        public Rect Area
        {
            get { return new Rect(new Point(X, Y), new Size(Size, Size)); }
        }

        public Rectangle Rect { get; }
    }
}
