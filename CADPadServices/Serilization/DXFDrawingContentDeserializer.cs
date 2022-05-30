using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Interfaces;
using netDxf;
using netDxf.Collections;
using netDxf.Header;
using System.Collections.Generic;
using System.IO;

namespace CADPadServices.Serilization
{    
    public class DXFDrawingContentDeserializer : DrawingContentDeserializer
    {
        private IDrawing _currentDrawing;

        private Stream _stream;
        public DXFDrawingContentDeserializer(Stream stream)
        {
            _stream = stream;
        }

        public override void Read(IDrawing drawing)
        {
            try
            {                
                DxfDocument dxf = DxfDocument.Load(_stream);
                
                _currentDrawing = drawing;

                ReadLayers(dxf);

                var entities = new List<Entity>();
                entities.AddRange(ReadLines(dxf));
                entities.AddRange(ReadPolylines(dxf));
                entities.AddRange(ReadArcs(dxf));
                entities.AddRange(ReadCircles(dxf));
                entities.AddRange(ReadEllipses(dxf));

                foreach (var item in drawing.CurrentBlock)
                {
                    item.Erase();
                }

                foreach (var item in entities)
                {
                    drawing.CurrentBlock.AppendEntity(item);
                }
            }
            finally
            {
                if (_stream != null)
                    _stream.Close();
            }
        }

        void ReadLayers(DxfDocument doc)
        {
            var layersTable = _currentDrawing.Document.Database.layerTable;
            
            foreach (var layer in doc.Layers)
            {
                if (layersTable.Has(layer.Name)) continue;

                var tabLayer = new CADPadDB.TableRecord.Layer(layer.Name);
                tabLayer.name = layer.Name;
                tabLayer.color = MapDxfColor(layer.Color);
                tabLayer.lineWeight = MapDxfLineWeight(layer.Lineweight);
                tabLayer.Visible = layer.IsVisible;
                layersTable.Add(tabLayer);
            }
        }

        void AssignEntityLayer(Entity entity, string layerName)
        {
            if (_currentDrawing.Document.Database.layerTable.Has(layerName))
            {
                entity.LayerId = _currentDrawing.Document.Database.layerTable[layerName].id;
            }
        }

        CADPadDB.LineWeight MapDxfLineWeight(Lineweight lineWeight)
        {
            return (CADPadDB.LineWeight)lineWeight;
        }

        CADPadDB.Colors.CADColor MapDxfColor(AciColor aciColor)
        {
            return CADPadDB.Colors.CADColor.FromArgb(aciColor.R, aciColor.G, aciColor.B);
        }

        List<Entity> ReadLines(DxfDocument doc)
        {
            var entities = new List<Entity>();
            foreach (var line in doc.Entities.Lines)
            {
                entities.Add(ReadLine(line, 0, 0));
            }

            return entities;
        }

        Entity ReadLine(netDxf.Entities.Line line, double x, double y)
        {
            Line l = new Line();
            l.startPoint = new CADPoint(line.StartPoint.X + x, line.StartPoint.Y + y);
            l.endPoint = new CADPoint(line.EndPoint.X + x, line.EndPoint.Y + y);

            AssignEntityLayer(l, line.Layer.Name);
            l.Color = MapDxfColor(line.Color);

            return l;
        }

        List<Entity> ReadPolylines(DxfDocument doc)
        {
            var entities = new List<Entity>();
            foreach (var item in doc.Entities.Polylines2D)
            {
                var poly = ReadPolyline(item.Vertexes, item.IsClosed, 0, 0);
                AssignEntityLayer(poly, item.Layer.Name);
                poly.Color = MapDxfColor(item.Color);
                entities.Add(poly);
            }
            return entities;
        }

        Polyline ReadPolyline(List<netDxf.Entities.Polyline2DVertex> vertices, bool isClosed, double x, double y)
        {
            Polyline poly = new Polyline();
            for (int i = 0; i < vertices.Count; i++)
            {
                netDxf.Entities.Polyline2DVertex vertex =vertices[i];
                var point = new CADPoint(vertex.Position.X + x, vertex.Position.Y + y);
                poly.AddVertexAt(i, point);
            }
            if (isClosed)
            {
                poly.closed = true;
            }
            return poly;
        }

        List<Entity> ReadCircles(DxfDocument doc)
        {
            var entities = new List<Entity>();
            foreach (var item in doc.Entities.Circles)
            {
                entities.Add(ReadCircle(item, 0, 0));
            }

            return entities;
        }

        Entity ReadCircle(netDxf.Entities.Circle circle, double x, double y)
        {
            Circle c = new Circle();
            c.center = new CADPoint(circle.Center.X + x, circle.Center.Y + y);
            c.radius = circle.Radius;

            AssignEntityLayer(c, circle.Layer.Name);
            c.Color = MapDxfColor(circle.Color);

            return c;
        }

        List<Entity> ReadEllipses(DxfDocument doc)
        {
            var entities = new List<Entity>();
            foreach (var item in doc.Entities.Ellipses)
            {
                entities.Add(ReadEllipse(item, 0, 0));
            }

            return entities;
        }

        /// <summary>
        /// TODO. take rotation into account
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        Entity ReadEllipse(netDxf.Entities.Ellipse ellipse, double x, double y)
        {
            Ellipse c = new Ellipse();
            c.center = new CADPoint(ellipse.Center.X + x, ellipse.Center.Y + y);
            c.RadiusX = ellipse.MajorAxis;
            c.RadiusY = ellipse.MinorAxis;

            AssignEntityLayer(c, ellipse.Layer.Name);
            c.Color = MapDxfColor(ellipse.Color);

            return c;
        }

        List<Entity> ReadArcs(DxfDocument doc)
        {
            var entities = new List<Entity>();
            foreach (var item in doc.Entities.Arcs)
            {
                entities.Add(ReadArc(item, 0, 0));
            }

            return entities;
        }

        Entity ReadArc(netDxf.Entities.Arc arc, double x, double y)
        {
            Arc c = new Arc();
            c.center = new CADPoint(arc.Center.X + x, arc.Center.Y + y);
            c.radius = arc.Radius;
            c.startAngle = Utils.DegreeToRadian(arc.StartAngle);
            c.endAngle = Utils.DegreeToRadian(arc.EndAngle);

            AssignEntityLayer(c, arc.Layer.Name);
            c.Color = MapDxfColor(arc.Color);

            return c;
        }
    }
}
