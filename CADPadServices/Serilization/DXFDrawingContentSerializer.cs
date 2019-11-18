using System;
using System.Collections.Generic;
using System.IO;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Interfaces;
using netDxf;


namespace CADPadServices.Serilization
{
    public class DXFDrawingContentSerializer : DrawingContentSerializer
    {
        private Stream _stream;
        public DXFDrawingContentSerializer(Stream stream)
        {
            _stream = stream;
        }

        public override void Write(IDrawing drawing)
        {
            try
            {
                DxfDocument doc = new DxfDocument();
                foreach (var entity in drawing.CurrentBlock)
                {
                    if (entity is Line)
                        WriteLine(doc, (Line)entity);
                    if (entity is Circle)
                        WriteCircle(doc, (Circle)entity);
                    if (entity is Ellipse)
                        WriteEllipse(doc, (Ellipse)entity);
                    if (entity is Arc)
                        WriteArc(doc, (Arc)entity);
                    if (entity is Polyline)
                        WritePolyline(doc, (Polyline)entity);
                }

                doc.Save(_stream);
            }
            finally
            {
                if (_stream != null)
                    _stream.Close();
            }

          
        }

        private void WriteLine(DxfDocument doc, Line line)
        {
            doc.AddEntity(new netDxf.Entities.Line(new Vector2(line.startPoint.X, line.startPoint.Y),
                new Vector2(line.endPoint.X, line.endPoint.Y)));
        }
        private void WriteCircle(DxfDocument doc, Circle circle)
        {
            doc.AddEntity(new netDxf.Entities.Circle(new Vector2(circle.center.X, circle.center.Y),
               circle.radius));
        }
        private void WriteEllipse(DxfDocument doc, Ellipse ellipse)
        {
            //Todo
            doc.AddEntity(new netDxf.Entities.Ellipse(new Vector2(ellipse.center.X, ellipse.center.Y),
               ellipse.RadiusX, ellipse.RadiusY));
        }
        private void WriteArc(DxfDocument doc, Arc arc)
        {
            var radius = CADPoint.Distance(arc.center, arc.startPoint);
            doc.AddEntity(new netDxf.Entities.Arc(new Vector2(arc.center.X, arc.center.Y),
             radius, arc.startAngle, arc.endAngle));
        }
        private void WritePolyline(DxfDocument doc, Polyline polyline)
        {
            var points = new List<netDxf.Entities.LwPolylineVertex>();
            for (int i = 0; i < polyline.NumberOfVertices; i++)
            {
                points.Add(new netDxf.Entities.LwPolylineVertex(polyline.GetPointAt(i).X, polyline.GetPointAt(i).Y));
            }
            doc.AddEntity(new netDxf.Entities.LwPolyline(points, polyline.closed));
        }
    }
}
