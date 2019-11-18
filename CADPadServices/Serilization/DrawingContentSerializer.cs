using CADPadServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CADPadServices.Serilization
{
    public abstract class DrawingContentSerializer
    {
        public abstract void Write(IDrawing drawing);
    }
}
