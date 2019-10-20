using CADPadDB;
using CADPadServices.ESelection;

namespace CADPadServices.Interfaces
{

    public interface ISelectionBoxVisual : IDrawingVisual
    {
        void Draw(SelectRectangle box);
        void Clear();
    }
}