using CADPadDB.Maths;

namespace CADPadDB
{
    public interface IDrawingVisual
    {

        void Open();
        void Close();

        void DrawCircle(CADPoint center, double radius, bool mdoelToCanvas = true);

        void DrawLine(CADPoint startPoint, CADPoint endPoint, bool mdoelToCanvas = true);

        void DrawRectangle(CADPoint position, double width, double height, bool mdoelToCanvas = true);
        void DrawXLine(CADPoint basePoint, CADVector direction, bool mdoelToCanvas = true);
        void DrawRay(CADPoint basePoint, CADVector direction, bool mdoelToCanvas = true);
        void DrawArc(CADPoint center, double radius, double startAngle, double endAngle, bool mdoelToCanvas = true);
        void DrawEllipse(CADPoint center, double radiusX, double radiusY, bool mdoelToCanvas = true);

        void SetColor(Colors.CADColor color);
    }
}