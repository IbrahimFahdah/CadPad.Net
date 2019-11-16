using System;
using System.Windows.Media;
using CADPadDB;
using CADPadDB.Maths;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadWPF.Control.Visuals
{
    internal class SelectBoxVisual : CanvasDrawingVisual, ISelectBoxVisual
    {
        public SelectBoxVisual(IDrawing drawing) : base(drawing)
        {

        }

        public void Draw(ISelectBox box)
        {
            Open();
            if (box.SelectionMode == SelectBox.SelectMode.Window)
            {
                Pen = new Pen(new SolidColorBrush(Colors.Green), 1);
            }
            else
            {
                Pen = new Pen(new SolidColorBrush(Colors.Blue), 1);
                Pen.DashStyle = DashStyles.Dash;
            }

            var p1 = box.StartPoint;
            var p2 = box.EndPoint;
            var w = Math.Abs(p1.X - p2.X);
            var h = Math.Abs(p1.Y - p2.Y);

            DrawRectangle(new CADPoint(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y)), w,h,false);
            Close();
        }

        public void Clear()
        {
            Open();
            Close();
            //  throw new System.NotImplementedException();
        }
    }
}