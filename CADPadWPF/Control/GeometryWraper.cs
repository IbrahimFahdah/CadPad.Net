using System.Windows;
using System.Windows.Media;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Interfaces;

namespace CADPadWPF.Control
{
    public class GeometryWraper : DrawingVisual, IDrawingVisual
    {

        public Entity Entity { get; set; } = null;
        public bool Selected { get; set; }

        IDrawing m_owner;
        public GeometryWraper(IDrawing owner)//, Entity geom)
        {
            m_owner = owner;
           // m_geometry = geom;
        }

        DrawingContext thisDC = null;
        public void Open()
        {
            thisDC = this.RenderOpen();
        }
        public void Close()
        {
            thisDC.Close();
            thisDC = null;
        }

        public IDrawingVisual Create()
        {
            return new GeometryWraper(m_owner);
        }

        public void Add()
        {
            m_owner.Canvas.AddGeometry( this);
        }

        public void Draw()
        {
            if (Entity != null)
            {
 
                Entity.Draw(this);
            }
        }

        public void DrawLine(CADPoint startPoint, CADPoint endPoint, bool mdoelToCanvas = true)
        {
            //  using (DrawingContext thisDC = this.RenderOpen())
            //  {
            var p1 = m_owner.ModelToCanvas(new CADPoint(startPoint.X, startPoint.Y));
            var p2 = m_owner.ModelToCanvas(new CADPoint(endPoint.X, endPoint.Y));

            Pen _pen = new Pen(new SolidColorBrush(Selected? Colors.Red: Colors.Teal), 2);
            thisDC.DrawLine(_pen, new Point(p1.X, p1.Y), new Point(p2.X, p2.Y));
            //   }

        }
    }
}
