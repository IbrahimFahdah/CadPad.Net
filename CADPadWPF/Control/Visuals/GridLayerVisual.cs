using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using CADPadDB.Maths;
using CADPadServices.Interfaces;
using CADPadWPF.Control.Canvas;

namespace CADPadWPF.Control.Visuals
{
    internal class GridLayerVisual : CanvasDrawingVisual, IGridLayerVisual
    {
        private Brush _fillBrush = null;
        public GridLayerVisual(IDrawing drawing) : base(drawing)
        {
            //seems to improve performance. when this used, EdgeMode.Aliased seems to have no effect.
            this.CacheMode = new BitmapCache();
        }

        public override  Brush FillBrush
        {
            get
            {
                if (_fillBrush == null)
                {
                    _fillBrush = new SolidColorBrush(Colors.Blue );
                    _fillBrush.Freeze();
                }
                return _fillBrush;
            }
        }

        public void Draw(IGridLayer box)
        {
            
            Open();
            CADPoint leftpoint= _drawing.CanvasToModel(new CADPoint(0,0));
            CADPoint rightpoint = _drawing.CanvasToModel(new CADPoint(((CADPadCanvas)_drawing.Canvas).ActualWidth, ((CADPadCanvas)_drawing.Canvas).ActualHeight));
            float gridX = 50;
            float gridY = 50;
            float gridscreensizeX = 20;
            float gridscreensizeY = 20;

            float left = (float)Math.Floor(leftpoint.X / gridX) * gridX;
            float top = (float)Math.Ceiling(leftpoint.Y / gridY) * gridY;
            float right = (float)Math.Ceiling(rightpoint.X / gridX) * gridX;
            float bottom = (float)Math.Floor(rightpoint.Y / gridY) * gridY;

            //while (left < right)                            //Draw vertical lines
            //{
            //    DrawLine(_drawing, thisDC, Pen, new CADPoint(left, leftpoint.Y), new CADPoint(left, rightpoint.Y), true);

            //    left += gridX;
            //}
            //while (bottom < top)                            //Draw vertical lines
            //{
            //    DrawLine(_drawing, thisDC, Pen, new CADPoint(leftpoint.X, bottom), new CADPoint(rightpoint.X, bottom), true);

            //    bottom += gridY;
            //}
            int count = 0;

            //PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();

            //var myPathFigureCollection = new PathFigureCollection();
            //        PathFigure myPathFigure = new PathFigure();
            //        myPathFigure.StartPoint = new Point(0,0);
            for (float x = left; x <= right; x += gridX)
            {
                for (float y = bottom; y <= top; y += gridY)
                {
                //   DrawCircle(_drawing, thisDC, null, Pen,new CADPoint(x, y),20,20, true);


                    //myPathSegmentCollection.Add(new ArcSegment(new CADPoint(x, y).AsWPF(), new Size(1, 1), 15, true, SweepDirection.Clockwise, true));
                    //myPathFigure.Segments = myPathSegmentCollection;
            
                  
                    count++;
                }
            }

            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;
            using (StreamGeometryContext ctx = geometry.Open())
            {
                for (float x = left; x <= right; x += gridX)
                {
                    for (float y = bottom; y <= top; y += gridY)
                    {
                        DrawCircle(_drawing, thisDC, null, Pen, new CADPoint(x, y), 20, 20, true);


                        count++;
                    }
                }
            }
            Path myPath = new Path();

            geometry.Freeze();

            thisDC.DrawGeometry(null, Pen, geometry);
            //myPathFigureCollection.Add(myPathFigure);
            //  PathGeometry myPathGeometry = new PathGeometry();
            //  myPathGeometry.Figures = myPathFigureCollection;
            //  myPathGeometry.Freeze();
            //  thisDC.DrawGeometry(null, Pen, myPathGeometry);
            //  //for (int i = 0; i < 1000; i++)
            //{
            //DrawXLine(_drawing, thisDC, Pen, new CADPoint(i*200,0), new CADVector(0,1), true);

            //}
            //for (int i = 0; i < 1000; i++)
            //{
            //    DrawXLine(_drawing, thisDC, Pen, new CADPoint(0, i * 200), new CADVector(1,0), true);

            //}
            //if (box.selectMode == SelectRectangle.SelectMode.Window)
            //{
            //    Pen = new Pen(new SolidColorBrush(Colors.Green), 1);
            //}
            //else
            //{
            //    Pen = new Pen(new SolidColorBrush(Colors.Blue), 1);
            //    Pen.DashStyle = DashStyles.Dash;
            //}

            //var p1 = box.startPoint;
            //var p2 = box.endPoint;
            //var w = Math.Abs(p1.X - p2.X);
            //var h = Math.Abs(p1.Y - p2.Y);

            //DrawRectangle(new CADPoint(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y)), w, h, false);

            Close();
        }
    }
}
