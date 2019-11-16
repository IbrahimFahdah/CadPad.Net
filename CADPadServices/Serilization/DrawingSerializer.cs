using CADPadServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CADPadServices.Serilization
{
    public abstract class DrawingSerializer
    {
        public abstract void Write(IDrawing drawing);
    }
}
