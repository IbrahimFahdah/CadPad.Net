using CADPadDB;
using CADPadDB.Maths;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Controllers
{
    public class Cursor
    {
        protected IDrawing _drawing = null;
        public int Length { get; set; } = 20;

        public Cursor(IDrawing drawing)
        {
            _drawing = drawing;
        }

        private ICursorVisual _cursorVisual = null;

        public ICursorVisual CursorVisual
        {
            get
            {
                if (_cursorVisual == null)
                {
                    _cursorVisual = _drawing.GetCursorVisual();
                }
                return _cursorVisual;
            }
            set => _cursorVisual = value;
        }

        public void Draw(PointerModes mode, CADPoint _loc, double pickupBox_side, bool show)
        {
            CursorVisual.Draw(mode, _loc, Length, pickupBox_side, show);
            //CursorVisual.Open();
            //var loc = _drawing.ModelToCanvas(_loc);
            //if (mode == PointerModes.Default || mode == PointerModes.Select)
            //{
            //    var p = new CADPoint(loc.X - pickupBox_side / 2, loc.Y - pickupBox_side / 2);
            //    CursorVisual.DrawRectangle(p, pickupBox_side, pickupBox_side, false);
            //}

            //if (mode == PointerModes.Default || mode == PointerModes.Locate)
            //{
            //    var p1 = new CADPoint(loc.X - Length / 2, loc.Y);
            //    var p2 = new CADPoint(loc.X + Length / 2, loc.Y);
            //    CursorVisual.DrawLine(p1, p2, false);
            //    p1 = new CADPoint(loc.X, loc.Y - Length / 2);
            //    p2 = new CADPoint(loc.X, loc.Y + Length / 2);
            //    CursorVisual.DrawLine(p1, p2, false);
            //}
            //CursorVisual.Close();

        }
    }
}