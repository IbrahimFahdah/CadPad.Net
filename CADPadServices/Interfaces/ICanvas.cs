using System.Collections.Generic;
using CADPadDB;

namespace CADPadServices.Interfaces
{
    public interface ICanvas
    {
        IDrawing Drawing { get; }

        List<ICADEnitiyVisual> Geometries { get; set; }

        ICADEnitiyVisual CreateCADEnitiyVisual();
        IDrawingVisual CreateVisual();

        ICursorVisual CursorVisual { get; set; }
        ISelectBoxVisual SelectBoxVisual { get; set; }
        IGridLayerVisual GridLayerVisual { get; set; }

        void AddVisual(IDrawingVisual dv);

        void RemoveVisual(IDrawingVisual dv);

        void Redraw();
        void ResetGrips();

        void ClearVisualGrips(IDrawingVisual associatedVisual);
    }
}
