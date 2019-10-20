using System.Windows;
using System.Windows.Media;
using CADPadDB;
using CADPadDB.Maths;
using CADPadServices.Interfaces;

namespace CADPadWPF.Control.Visuals
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
                    _pen = new Pen(new SolidColorBrush(Colors.Black), 2);
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

        public virtual void DrawLine(CADPoint startPoint, CADPoint endPoint, bool mdoelToCanvas = true)
        {
            DrawLine(_drawing, thisDC, Pen, startPoint, endPoint, mdoelToCanvas);
        }

        public virtual void DrawRectangle(CADPoint startPoint, double width, double height, bool mdoelToCanvas = true)
        {
            DrawRectangle(_drawing, thisDC, FillBrush, Pen, startPoint, width, height, mdoelToCanvas);
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
            var p1 = startPoint;
            if (mdoelToCanvas)
            {
                owner.ModelToCanvas(new CADPoint(startPoint.X, startPoint.Y));
            }
            thisDC.DrawRectangle(fillBrush, pen, new Rect(new Point(p1.X, p1.Y), new Size(width, height)));
        }
    }
}