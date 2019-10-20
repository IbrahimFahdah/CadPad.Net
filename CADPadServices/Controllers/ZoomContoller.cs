using System.Collections.Generic;
using CADPadDB.Maths;
using CADPadServices.Commands;
using CADPadServices.Interfaces;

namespace CADPadServices.Controllers
{
    public class ZoomContoller : IZoomContoller
    {
        private Drawing _drawing;

        private static List<double> m_ScaleList = new List<double>()
                                                  {
                                                      0.1,
                                                      0.3,
                                                      0.5,
                                                      0.75,
                                                      0.90,
                                                      1,
                                                      1.1,
                                                      1.2,
                                                      1.5,
                                                      1.75,
                                                      2,
                                                      4
                                                  };
        public ZoomContoller(Drawing drawing)
        {
            this._drawing = drawing;
        }

        public double Scale { get; set; }

        public IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            return null;
        }

        public IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            return null;
        }

        public IEventResult OnMouseMove(IMouseEventArgs e)
        {
            return null;
        }

        public IEventResult OnMouseWheel(IMouseWheelEventArgs e)
        {
            Zoom(e); return null;
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

        private void Zoom(IMouseWheelEventArgs e)
        {
            CADPoint globalPnt_UnderMouse = _drawing.CanvasToModel(new CADPoint(e.X, e.Y));
            CADPoint localPnt_UnderMouse = new CADPoint(e.X, e.Y);
            //
            // Mouse wheel was spinned.
            // Calculate new scale.
            double currentScale = Scale;
            //
            // Delta > 0 - scrolling to user
            // Delta < 0 - from user
            if (e.Delta > 0)
            {
                // zoom out
                if (currentScale >= m_ScaleList[m_ScaleList.Count - 1])
                    return;

                foreach (double rVal in m_ScaleList)
                {
                    if (currentScale < rVal)
                    {
                        Scale = rVal;
                        break;
                    }
                }
            }
            else
            {
                // zoom in
                if (currentScale <= m_ScaleList[0])
                    return;

                int iIndex = m_ScaleList.Count - 1;
                while (iIndex >= 0)
                {
                    if (currentScale > m_ScaleList[iIndex])
                    {
                        Scale = m_ScaleList[iIndex];
                        break;
                    }
                    --iIndex;
                }
            }

            //
            // Need to save cursor position - cursor should been placed over same point in global coordiantes.
            globalPnt_UnderMouse.X *= Scale;
            globalPnt_UnderMouse.Y *= Scale;

            // reverse Y
            // read comment in GetLocalPoint()
            globalPnt_UnderMouse.Y *= -1;

            _drawing.Origin = globalPnt_UnderMouse - localPnt_UnderMouse;

            // reverse Y
            _drawing.Origin.Y *= -1;

            // m_TempOffsetVector.X = 0;
            //  m_TempOffsetVector.Y = 0;

            _drawing.Canvas.Redraw();
        }
    }
}