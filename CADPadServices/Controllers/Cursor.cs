using CADPadDB;
using CADPadDB.Maths;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Controllers
{
    public class Cursor : ICursor
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
        }
    }
}