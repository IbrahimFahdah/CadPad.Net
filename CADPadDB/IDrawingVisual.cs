
using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadDB
{
    public interface IDrawingVisual
    {
        void Draw();
        void DrawLine(CADPoint startPoint, CADPoint endPoint, bool mdoelToCanvas = true);

        //void DrawXLine(Vector2 basePoint, Vector2 direction, bool mdoelToCanvas = true);

        //void DrawRay(Vector2 basePoint, Vector2 direction, bool mdoelToCanvas = true);

        //void DrawCircle(Vector2 center, double radius, bool mdoelToCanvas = true);

        //void DrawArc(Vector2 center, double radius, double startAngle, double endAngle, bool mdoelToCanvas = true);

        //void DrawRectangle(Vector2 position, double width, double height, bool mdoelToCanvas = true);

        //Vector2 DrawText(Vector2 position, string text, double height, string font, TextAlignment textAlign, bool mdoelToCanvas = true);
        void Open();
        void Close();

        Entity Entity { get; set; }
        bool Selected { get; set; }
    }
}
