using CADPadDB.Maths;
using CADPadServices.Enums;

namespace CADPadServices.Interfaces
{
    public interface ICursor
    {
        int Length { get; set; }

        void Draw(PointerModes mode, CADPoint _loc, double pickupBox_side, bool show);
    }
}