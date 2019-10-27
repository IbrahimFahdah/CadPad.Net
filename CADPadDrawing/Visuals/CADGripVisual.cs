using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CADPadDB;
using CADPadDB.Maths;
using CADPadServices.Interfaces;

namespace CADPadDrawing.Visuals
{
    internal class CADGripVisual : CanvasDrawingVisual
    {
       
        private GripPoint _gripPoint ;
        private Brush _fillBrush;

        public CADGripVisual(IDrawing drawing, GripPoint gripPoint, IDrawingVisual associatedVisual) :base(drawing)
        {
            _gripPoint = gripPoint;
            AssociatedVisual = associatedVisual;
        }

        public IDrawingVisual AssociatedVisual { get; set; }
        public override Brush FillBrush
        {
            get
            {
                if (_fillBrush == null)
                {
                    _fillBrush = new SolidColorBrush(Colors.Blue);
                }
                return _fillBrush;
            }
        }

        public void Draw()
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                Vector vec = new Vector(4.0, 4.0);
                Point pnt = _drawing.ModelToCanvas(_gripPoint.Position).AsWPF() ;
                Point pnt1 = pnt - vec;
                Point pnt2 = pnt + vec;
                Rect rect = new Rect(pnt1, pnt2);

                dc.DrawRectangle(FillBrush, Pen, rect);
            }
        }

     
    }
}