using CADPadDB;
using CADPadServices.ESelection;

namespace CADPadServices.Interfaces
{
    public interface ISelectBoxVisual : IDrawingVisual
    {
        void Draw(ISelectBox box);
        void Clear();
    }
}