using System.Windows.Input;
using CADPadServices.Interfaces;

namespace CADPadDrawing.Canvas
{
    internal class CanvasKeyEventArgs : IKeyEventArgs
    {
        readonly CADPadCanvas _parent;
        public CanvasKeyEventArgs(KeyEventArgs e, CADPadCanvas parent)
        {
            _parent = parent;
            InnerArgs = e;
        }
        public object InnerArgs { get; set; }


        public bool IsEscape  => ((KeyEventArgs)InnerArgs).Key==Key.Escape;
        public bool IsDelete => ((KeyEventArgs)InnerArgs).Key == Key.Delete;
    }
}
