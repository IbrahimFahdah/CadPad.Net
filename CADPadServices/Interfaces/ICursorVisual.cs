using CADPadDB;
using CADPadDB.Maths;
using CADPadServices.Enums;

namespace CADPadServices.Interfaces
{
    public interface ICursorVisual : IDrawingVisual
    {
        void Draw(PointerModes mode, CADPoint _loc, double Length, double pickupBox_side, bool show);
        
    }
}