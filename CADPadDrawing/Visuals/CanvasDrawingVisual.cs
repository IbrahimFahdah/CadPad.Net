using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CADPadDB;
using CADPadDB.Maths;
using CADPadServices.Interfaces;

namespace CADPadDrawing.Visuals
{

    public class CanvasDrawingVisual : DrawingVisual, IDrawingVisual
    {
        protected IDrawing _drawing;
        private Pen _pen;
        private Brush _fillBrush = null;
        protected DrawingContext thisDC = null;

        public CanvasDrawingVisual(IDrawing drawing)
        {
            _drawing = drawing;
        }

        public void Open()
        {
            thisDC = this.RenderOpen();
        }
        public void Close()
        {
            thisDC.Close();
            thisDC = null;
        }

        public virtual Pen Pen
        {
            get
            {
                if (_pen == null)
                {
                    var b=new SolidColorBrush(Colors.White);
           
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

        public virtual Brush FillBrush
        {
            get
            {
                if (_fillBrush == null)
                {
                    _fillBrush = new SolidColorBrush(Colors.Transparent);
                }
                return _fillBrush;
            }
        }

        public void DrawCircle(CADPoint center, double radius, bool mdoelToCanvas = true)
        {
            DrawCircle(_drawing, thisDC, FillBrush, Pen, center, radius, radius, mdoelToCanvas);
        }

        public void DrawEllipse(CADPoint center, double radiusX, double radiusY, bool mdoelToCanvas = true)
        {
            DrawCircle(_drawing, thisDC, FillBrush, Pen, center, radiusX, radiusY, mdoelToCanvas);
        }


        public virtual void DrawLine(CADPoint startPoint, CADPoint endPoint, bool mdoelToCanvas = true)
        {
            DrawLine(_drawing, thisDC, Pen, startPoint, endPoint, mdoelToCanvas);
        }

        public virtual void DrawRectangle(CADPoint startPoint, double width, double height, bool mdoelToCanvas = true)
        {
            DrawRectangle(_drawing, thisDC, FillBrush, Pen, startPoint, width, height, mdoelToCanvas);
        }

        public virtual void DrawXLine(CADPoint basePoint, CADVector direction, bool mdoelToCanvas = true)
        {
            DrawXLine(_drawing, thisDC,  Pen, basePoint, direction, mdoelToCanvas);
        }

        public virtual void DrawRay(CADPoint basePoint, CADVector direction, bool mdoelToCanvas = true)
        {
            DrawRay(_drawing, thisDC, Pen, basePoint, direction, mdoelToCanvas);
        }
        public virtual void DrawArc(CADPoint center, double radius, double startAngle, double endAngle, bool mdoelToCanvas = true)
        {
            DrawArc(_drawing, thisDC,  Pen, center, radius, startAngle, endAngle, mdoelToCanvas);
        }

        protected static void DrawCircle(IDrawing drawing, DrawingContext thisDC, Brush fillBrush, Pen pen, CADPoint center, double radiusX, double radiusY, bool mdoelToCanvas)
        {
            var p = center;
            var rx = radiusX;
            var ry = radiusY;
            if (mdoelToCanvas)
            {
                p = drawing.ModelToCanvas(new CADPoint(center.X, center.Y));
                rx = drawing.ModelToCanvas(radiusX);
                ry = drawing.ModelToCanvas(radiusY);
            }
            thisDC.DrawEllipse(fillBrush, pen, new Point(p.X, p.Y), rx, ry);
        }


     

        /// <summary>
        /// https://stackoverflow.com/questions/16667072/how-to-draw-arc-with-radius-and-start-and-stop-angle
        /// </summary>
        /// <param name="drawing"></param>
        /// <param name="thisDC"></param>
        /// <param name="pen"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        /// <param name="mdoelToCanvas"></param>
        protected static void DrawArc(IDrawing drawing, DrawingContext thisDC, Pen pen, CADPoint center, double radius, double startAngle, double endAngle, bool mdoelToCanvas = true)
        {

            CADPoint centerInCanvas = mdoelToCanvas ? drawing.ModelToCanvas(center) : center;
            double radiusInCanvas = mdoelToCanvas ? drawing.ModelToCanvas(radius) : radius;

            double startAngleInCanvas = MathUtils.NormalizeRadianAngle(-startAngle);
            double endAngleInCanvas = MathUtils.NormalizeRadianAngle(-endAngle);

            //
            double angle = endAngle - startAngle;
            if (endAngle < startAngle)
            {
                angle += Utils.PI * 2;
            }

         
            if (radiusInCanvas > 0)
            {
                double a0 = startAngleInCanvas < 0 ? startAngleInCanvas + 2 * Math.PI : startAngleInCanvas;
                double a1 = endAngleInCanvas < 0 ? endAngleInCanvas + 2 * Math.PI : endAngleInCanvas;

                if (a1 < a0)
                    a1 += Math.PI * 2;

                SweepDirection d = SweepDirection.Counterclockwise;
                bool large;

                bool SmallAngle = false;
                if (SmallAngle)
                {
                    large = false;
                    d = (a1 - a0) > Math.PI ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;
                }
                else
                {
                    large = (Math.Abs(a1 - a0) < Math.PI);
                }

                Point p0 = centerInCanvas.AsWPF() + new Vector(Math.Cos(a0), Math.Sin(a0)) * radiusInCanvas;
                Point p1 = centerInCanvas.AsWPF() + new Vector(Math.Cos(a1), Math.Sin(a1)) * radiusInCanvas;

                List<PathSegment> segments = new List<PathSegment>
                                             {
                                                 new ArcSegment(p1, new Size(radiusInCanvas, radiusInCanvas), 0.0, large, d, true)
                                             };

                List<PathFigure> figures = new List<PathFigure>
                                           {
                                               new PathFigure(p0, segments, true)
                                               {
                                                   IsClosed = false
                                               }
                                           };

                thisDC.DrawGeometry(null, pen, new PathGeometry(figures, FillRule.EvenOdd, null)); 

            
            }
        }

        protected static void DrawLine(IDrawing owner, DrawingContext thisDC , Pen pen, CADPoint startPoint, CADPoint endPoint, bool mdoelToCanvas = true)
        {
            var p1 = startPoint;
            var p2 = endPoint;
            if (mdoelToCanvas)
            {
                p1 = owner.ModelToCanvas(new CADPoint(startPoint.X, startPoint.Y));
                p2 = owner.ModelToCanvas(new CADPoint(endPoint.X, endPoint.Y));
            }
            thisDC.DrawLine(pen, new Point(p1.X, p1.Y), new Point(p2.X, p2.Y));
        }

        protected static void DrawRectangle(IDrawing owner, DrawingContext thisDC, Brush fillBrush, Pen pen, CADPoint startPoint, double width, double height, bool mdoelToCanvas = true)
        {
            var p = startPoint;
            if (mdoelToCanvas)
            {
                p = owner.ModelToCanvas(new CADPoint(startPoint.X, startPoint.Y));
            }
            thisDC.DrawRectangle(fillBrush, pen, new Rect(new Point(p.X, p.Y), new Size(width, height)));
        }

        public static void DrawXLine(IDrawing owner, DrawingContext thisDC, Pen pen, CADPoint basePoint, CADVector direction, bool mdoelToCanvas = true)
        {
            CADVector dir;
            CADPoint basePnt;
            if (mdoelToCanvas)
            {
                basePnt = owner.ModelToCanvas(basePoint);
                CADPoint otherPnt = owner.ModelToCanvas(basePoint + direction);
                dir = (otherPnt - basePnt).normalized;
            }
            else
            {
                basePnt = basePoint;
                dir = direction;
                dir.Normalize();
            }

            double xk = double.MinValue;
            double yk = double.MinValue;
            if (dir.Y != 0)
            {
                double k = basePnt.Y / dir.Y;
                xk = basePnt.X - k * dir.X;
            }
            if (dir.X != 0)
            {
                double k = basePnt.X / dir.X;
                yk = basePnt.Y - k * dir.Y;
            }

            if (xk > 0
                || (xk == 0 && dir.X * dir.Y >= 0))
            {
                CADPoint spnt = new CADPoint(xk, 0);
                if (dir.Y < 0)
                {
                    dir = -dir;
                }
                CADPoint epnt = spnt + 10000 * dir;

                DrawLine(owner, thisDC, pen,
                    new CADPoint(spnt.X, spnt.Y),
                    new CADPoint(epnt.X, epnt.Y), false);
            }
            else if (yk > 0
                     || (yk == 0 && dir.X * dir.Y >= 0))
            {
                CADPoint spnt = new CADPoint(0, yk);
                if (dir.X < 0)
                {
                    dir = -dir;
                }
                CADPoint epnt = spnt + 10000 * dir;

                DrawLine(owner, thisDC, pen,
                    new CADPoint(spnt.X, spnt.Y),
                    new CADPoint(epnt.X, epnt.Y), false);

            }
        }
        public static void DrawRay(IDrawing owner, DrawingContext thisDC, Pen pen, CADPoint basePoint, CADVector direction, bool mdoelToCanvas = true)
        {
            CADVector dir;
            CADPoint basePnt;
            if (mdoelToCanvas)
            {
                basePnt = owner.ModelToCanvas(basePoint);
                CADPoint otherPnt = owner.ModelToCanvas(basePoint + direction);
                dir = (otherPnt - basePnt).normalized;
            }
            else
            {
                basePnt = basePoint;
                dir = direction;
                dir.Normalize();
            }

            double xk = double.MinValue;
            double yk = double.MinValue;
            if (basePnt.X > 0 && basePnt.X < 10000
                && basePnt.Y > 0 && basePnt.Y < 10000)
            {
                xk = 1;
                yk = 1;
            }
            else
            {
                if (dir.Y != 0)
                {
                    double k = -basePnt.Y / dir.Y;
                    if (k >= 0)
                    {
                        xk = basePnt.X + k * dir.X;
                    }
                }
                if (dir.X != 0)
                {
                    double k = -basePnt.X / dir.X;
                    if (k >= 0)
                    {
                        yk = basePnt.Y + k * dir.Y;
                    }
                }

            }

            if (xk > 0
                || (xk == 0 && dir.X * dir.Y >= 0)
                || yk > 0
                || (yk == 0 && dir.X * dir.Y >= 0))
            {

                CADPoint epnt = basePnt + 10000 * dir;

                DrawLine(owner, thisDC, pen,
                   new CADPoint(basePnt.X,basePnt.Y),
                   new CADPoint(epnt.X, epnt.Y), false);
            }
        }

    }
}