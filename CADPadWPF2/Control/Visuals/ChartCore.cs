using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CADPadWPF.Control.Visuals
{
    class ChartCore : DrawingVisual
    {
        ImageBrush ib;

        public ChartCore()
        {
            Rectangle r = new Rectangle();
            r.Width = 32;
            r.Height = 32;
            r.Fill = new LinearGradientBrush(Colors.Yellow, Colors.RoyalBlue, 13.37);

            r.Measure(new Size(32, 32));
            r.Arrange(new Rect(new Size(32, 32)));
            r.UpdateLayout();

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)r.ActualWidth, (int)r.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
            rtb.Render(r);

            rtb.Freeze();

            ib = new ImageBrush(rtb);
            ib.Freeze();
        }




        public void render()
        {
            Random r = new Random(System.DateTime.Now.Millisecond);
            DrawingContext dc = RenderOpen();

            for (int n = 0; n < 2000; ++n)
            {
                Rect rect = new Rect(new Point(r.NextDouble() * 700, r.NextDouble() * 700), new Size(20, 20));
                dc.DrawRectangle(ib, null, rect);
            }

            dc.Close();
        }
    }
}