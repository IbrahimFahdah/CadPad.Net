using CADPadDB.Maths;

namespace CADPadDB
{
    public interface IDrawingVisual
    {
      
        void DrawLine(CADPoint startPoint, CADPoint endPoint, bool mdoelToCanvas = true);

        void DrawRectangle(CADPoint position, double width, double height, bool mdoelToCanvas = true);

        void Open();
        void Close();

     
    }
}
