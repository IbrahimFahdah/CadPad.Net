using CADPadDB;

namespace CADPadServices.Interfaces
{
    public interface IGridLayerVisual : IDrawingVisual
    {
        void Draw(IGridLayer grid);

    }
}