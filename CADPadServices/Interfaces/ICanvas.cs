using System.Collections.Generic;
using CADPadDB;

namespace CADPadServices.Interfaces
{
    public interface ICanvas
    {
        IDrawing Drawing { get; }

        List<IDrawingVisual> Geometries { get; set; }

        IDrawingVisual CreateGeometryWraper();
        void AddGeometry(IDrawingVisual dv);
        void RemoveGeometry(IDrawingVisual dv);

        void Redraw();
    }
}
