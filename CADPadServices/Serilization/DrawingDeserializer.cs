using CADPadServices.Interfaces;

namespace CADPadServices.Serilization
{
    public abstract class DrawingDeserializer
    {
        public abstract void Read(IDrawing drawing);
    }
}
