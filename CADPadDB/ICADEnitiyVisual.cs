using System.Collections.Generic;
using CADPadDB.CADEntity;

namespace CADPadDB
{
    public interface ICADEnitiyVisual: IDrawingVisual
    {
        void Draw();
        Entity Entity { get; set; }
        bool Selected { get; set; }
        List<GripPoint> GetGripPoints();
    }
}