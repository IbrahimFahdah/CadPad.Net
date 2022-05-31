using System;
using System.Windows.Media;
using CADPadDB.Maths;
using CADPadServices.Enums;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadDrawing.Visuals
{
    internal class CADCursorVisual : CanvasDrawingVisual, ICursorVisual
    {
        public const int DisabledCursorSize = 10;
        public CADCursorVisual(IDrawing drawing) : base(drawing)
        {

        }

        public void Draw(PointerModes mode, CADPoint _loc, double Length, double pickupBox_side, bool show)
        {
            Open();
            if(!show)
            {
                Close();
                return;
            }

            var loc = _drawing.ModelToCanvas(_loc);
            if (mode == PointerModes.Default || mode == PointerModes.Select)
            {
                var p = new CADPoint(loc.X - pickupBox_side / 2, loc.Y - pickupBox_side / 2);
                DrawRectangle(p, pickupBox_side, pickupBox_side, false);
            }

            if (mode == PointerModes.Default || mode == PointerModes.Locate)
            {
                var p1 = new CADPoint(loc.X - Length / 2, loc.Y);
                var p2 = new CADPoint(loc.X + Length / 2, loc.Y);
                DrawLine(p1, p2, false);
                p1 = new CADPoint(loc.X, loc.Y - Length / 2);
                p2 = new CADPoint(loc.X, loc.Y + Length / 2);
                DrawLine(p1, p2, false);
            }

            if (mode == PointerModes.Disabled)
            {
                var p1 = new CADPoint(loc.X - DisabledCursorSize / 2, loc.Y);
                var p2 = new CADPoint(loc.X + DisabledCursorSize / 2 - 1, loc.Y);
                DrawLine(p1, p2, false);
                p1 = new CADPoint(loc.X, loc.Y - DisabledCursorSize / 2);
                p2 = new CADPoint(loc.X, loc.Y + DisabledCursorSize / 2 - 1);
                DrawLine(p1, p2, false);
            }

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