using System.Windows.Input;
using CADPadServices.Interfaces;

namespace CADPadDrawing.Canvas
{
    internal class CanvasMouseWheelEventArgs : CanvasMouseEventArgs, IMouseWheelEventArgs
    {
      
        public CanvasMouseWheelEventArgs(MouseWheelEventArgs e, CADPadCanvas parent): base(e, parent)
        {
            Delta = e.Delta;
        }

        public int Delta { get; }
    }
}