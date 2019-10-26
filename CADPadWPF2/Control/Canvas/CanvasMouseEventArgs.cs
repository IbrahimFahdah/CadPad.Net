using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CADPadServices.Interfaces;

namespace CADPadWPF.Control.Canvas
{
    internal class CanvasMouseEventArgs : IMouseEventArgs
    {
        readonly CADPadCanvas _parent;
        public CanvasMouseEventArgs(MouseEventArgs e, CADPadCanvas parent )
        {
            _parent = parent;
            InnerArgs = e;
        }
        public object InnerArgs { get; set; }
        public bool IsMiddlePressed => ((MouseEventArgs)InnerArgs).MiddleButton == MouseButtonState.Pressed;
        public bool IsLeftPressed => ((MouseEventArgs)InnerArgs).LeftButton == MouseButtonState.Pressed;
        public bool IsRightPressed => ((MouseEventArgs)InnerArgs).RightButton == MouseButtonState.Pressed;
        public double X => ((MouseEventArgs)InnerArgs).GetPosition(_parent).X;
        public double Y => ((MouseEventArgs)InnerArgs).GetPosition(_parent).Y;

    }
}
