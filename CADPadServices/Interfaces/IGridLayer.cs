using CADPadDB.Colors;
using CADPadServices.Enums;

namespace CADPadServices.Interfaces
{
    public interface IGridLayer
    {
       int SpacingX { get; set; }
       int SpacingY { get; set; }

       int MinSize { get; set; }

        GridStyle GridStyle { get; set; }

       CADColor Color { get; set; }

       double CrossSize { get; set; }
        void Draw();
    }
}
