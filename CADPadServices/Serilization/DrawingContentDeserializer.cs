using CADPadServices.Interfaces;

namespace CADPadServices.Serilization
{
    public abstract class DrawingContentDeserializer
    {
        public abstract void Read(IDrawing drawing);
    }
}
