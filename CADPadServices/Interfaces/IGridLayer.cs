using CADPadDB;
using CADPadDB.Colors;
using CADPadDB.Maths;
using CADPadServices.Enums;
using System.Collections.Generic;

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
        List<ObjectSnapPoint> GetSnapPoints(CADPoint posInModel);
    }
}
