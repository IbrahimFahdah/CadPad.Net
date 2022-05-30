using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using CADPadDB.Maths;
using CADPadDrawing.Canvas;
using CADPadServices.Interfaces;

namespace CADPadDrawing.Visuals
{
    internal class GridLayerVisual : CanvasDrawingVisual, IGridLayerVisual
    {
        private Pen _pen;
        private Brush _fillBrush = null;
        public GridLayerVisual(IDrawing drawing) : base(drawing)
        {
            //seems to improve performance. when this used, EdgeMode.Aliased seems to have no effect.
            this.CacheMode = new BitmapCache();
        }

        public override Pen Pen
        {
            get
            {
                if (_pen == null)
                {
                    var b = new SolidColorBrush(_drawing.GridLayer.Color.ConvertToWPF());

                    RenderOptions.SetCachingHint(b, CachingHint.Cache);
                    b.Freeze();
                    _pen = new Pen(b, 1);
                    //critical for good performance

                    _pen.Freeze();
                }
                return _pen;
            }
            set
            {
                _pen = value;
            }
        }

        public override Brush FillBrush
        {
            get
            {
                if (_fillBrush == null)
                {
                    _fillBrush = new SolidColorBrush(Colors.Blue);
                    _fillBrush.Freeze();
                }
                return _fillBrush;
            }
        }

        public void Draw(IGridLayer grid)
        {
            Open();
            
            if (grid.GridStyle == CADPadServices.Enums.GridStyle.None)
            {
                Close();
                return;
            }

            CADPoint leftpoint = _drawing.CanvasToModel(new CADPoint(0, 0));
            CADPoint rightpoint = _drawing.CanvasToModel(new CADPoint(((CADPadCanvas)_drawing.Canvas).ActualWidth, ((CADPadCanvas)_drawing.Canvas).ActualHeight));
            double gridX = grid.SpacingX;
            double gridY = grid.SpacingY;

            var left = Math.Floor(leftpoint.X / gridX) * gridX;
            var top = Math.Ceiling(leftpoint.Y / gridY) * gridY;
            var right = Math.Ceiling(rightpoint.X / gridX) * gridX;
            var bottom = Math.Floor(rightpoint.Y / gridY) * gridY;

            if (grid.GridStyle == CADPadServices.Enums.GridStyle.Lines)
            {
                while (left < right)
                {
                    DrawLine(_drawing, thisDC, Pen, new CADPoint(left, leftpoint.Y), new CADPoint(left, rightpoint.Y), true);

                    left += gridX;
                }
                while (bottom < top)
                {
                    DrawLine(_drawing, thisDC, Pen, new CADPoint(leftpoint.X, bottom), new CADPoint(rightpoint.X, bottom), true);

                    bottom += gridY;
                }
            }
            else if (grid.GridStyle == CADPadServices.Enums.GridStyle.Circle)
            {

                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    for (double x = left; x <= right; x += gridX)
                    {
                        for (double y = bottom; y <= top; y += gridY)
                        {
                            DrawCircle(_drawing, thisDC, null, Pen, new CADPoint(x, y), 20, 20, true);
                        }
                    }
                }
                geometry.Freeze();

                thisDC.DrawGeometry(null, Pen, geometry);
            }
           
            Close();
        }
    }
}
