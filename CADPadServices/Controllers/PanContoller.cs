using CADPadDB.Maths;
using CADPadServices.Commands;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Controllers
{
    public class PanContoller: IPanContoller
    {
        private Drawing _drawing;

        CADPoint m_MiddleBtnPressed_Point = new CADPoint(0, 0);


        /// <summary>
        /// Temporary offset vector.
        /// It shows offset when mouse middle button is pressed and mouse is moved.
        /// In this case dont need to change Origin because when mouse will move again
        /// (with middle button pressed) need to deduct "old" temp offset and add "new" temp offset.
        /// But we cant calculate "old" temp offset because mouse position is new.
        /// </summary>
        private CADVector _tempOffsetVector = new CADVector(0.0, 0.0);
        public PanContoller(Drawing drawing)
        {
            this._drawing = drawing;
        }

        public IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                m_MiddleBtnPressed_Point = new CADPoint(e.X, e.Y);
            }
            return null;
        }

        public IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Released)
            {
                _drawing.Origin = _drawing.Origin + _tempOffsetVector;
                _tempOffsetVector.X = 0.0;
                _tempOffsetVector.Y = 0.0;
            }
            return null;
        }

        public IEventResult OnMouseMove(IMouseEventArgs e)
        {
            if (e.IsMiddlePressed)
            {
                var pnt = new CADPoint(e.X, e.Y);
                _tempOffsetVector = (m_MiddleBtnPressed_Point - pnt);
                _tempOffsetVector.Y *= -1;

                _drawing.Canvas.Redraw();

            }
            return null;
        }

        public IEventResult OnMouseWheel(IMouseWheelEventArgs e)
        {
            return null;
        }

        public IEventResult OnMouseDoubleClick(IMouseEventArgs e)
        {
            return null;
        }

        public IEventResult OnKeyDown(IKeyEventArgs KeyEventArgs)
        {
            return null;
        }

        public IEventResult OnKeyUp(IKeyEventArgs KeyEventArgs)
        {
            return null;
        }

        public CADVector GetOffset()
        {
            return _drawing.Origin + _tempOffsetVector;
        }
    }
}