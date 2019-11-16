using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Colors;
using CADPadDB.Maths;
using CADPadDB.TableRecord;
using CADPadServices.ESelection;

namespace CADPadServices.Interfaces
{

    public interface IDrawing: IMouseKeyReceiver
    {
        ICanvas Canvas { get; set; }

        CADColor AxesColor { get; set; }

        double AxesThickness { get; set; }
        double AxesLength { get; set; }
        double AxesTextSize { get; set; }

        double Scale { get; set; }

        ICoordinateAxes Axes { get; set; }
        IGridLayer GridLayer { get; set; }

        CADPoint MousePoint { get; set; }

        CADPoint ModelToCanvas(CADPoint p, bool bWithScale = true);

        CADPoint CanvasToModel(CADPoint p);

        double CanvasToModel(double p);
        double ModelToCanvas(double value);
        //void OnMouseDown(IMouseButtonEventArgs e);
        //void OnMouseUp(IMouseButtonEventArgs e);
        //void OnMouseMove(IMouseEventArgs e);


        T AppendEntity<T>(T entity, DBObjectState state = DBObjectState.Default, bool reUseVisual = false) where T : Entity;
        void RemoveEntity<T>(T entity) where T : Entity;

        Selections selections { get; }

        Document Document { get; }
        Block CurrentBlock { get; }
        IPointerContoller Pointer { get; set; }

        SelectBox CreateSelectRectangle();
        void ResetGrips();
        IDrawingVisual CreateTempVisual();

        ICursorVisual GetCursorVisual();

        ISelectBoxVisual GetSelectBoxVisual();

        IGridLayerVisual GetGridLayerVisual();
        void RemoveTempVisual(IDrawingVisual v);
        void RemoveUnconfirmed();
    }
}
