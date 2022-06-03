using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CADPadDB.Maths;

namespace CADPadDB.CADEntity
{

    public class Polyline : Entity
    {

        public override string ClassName
        {
            get { return "Polyline"; }
        }

        private List<CADPolyLine2DVertex> _vertices = new List<CADPolyLine2DVertex>();

        private List<CADPoint> _verticesAsPoints { get => _vertices.Select(a => a.Point).ToList(); }

        private bool _closed = false;

        public int NumberOfVertices
        {
            get
            {
                return _vertices.Count;
            }
        }

        public bool closed
        {
            get { return _closed; }
            set { _closed = value; }
        }


        public override void Draw(IDrawingVisual gd)
        {
            if (gd == null)
                return;

            gd.Open();
            gd.SetColor(ColorValue);
            int numOfVertices = NumberOfVertices;
            for (int i = 0; i < numOfVertices - 1; ++i)
            {
                var vertex = GetVertexAt(i);
                if (vertex.Bulge == 0)
                {
                    gd.DrawLine(vertex.Point, GetVertexAt(i + 1).Point);
                }
                else
                {
                    //TODO: MKI - implement conversion
                    var arc = Utils.BulgeToArc(vertex.Point, GetVertexAt(i + 1).Point, vertex.Bulge);
                    gd.DrawArc(arc.center, arc.radius, arc.startAngle, arc.endAngle);
                }                
            }

            if (closed
                && numOfVertices > 2)
            {
                gd.DrawLine(GetVertexAt(numOfVertices - 1).Point, GetVertexAt(0).Point);
            }
            gd.Close();
        }

        public void AddVertexAt(int index, CADPolyLine2DVertex vertex)
        {
            _vertices.Insert(index, vertex);
        }

        public void AddVertexAt(int index, CADPoint point)
        {
            _vertices.Insert(index, new CADPolyLine2DVertex(point));
        }

        public void RemoveVertexAt(int index)
        {
            _vertices.RemoveAt(index);
        }

        public CADPolyLine2DVertex GetVertexAt(int index)
        {
            return _vertices[index];
        }

        public void SetVertexAt(int index, CADPolyLine2DVertex vertex)
        {
            _vertices[index] = vertex;
        }

        public void SetPointAt(int index, CADPoint point)
        {
            _vertices[index].Point = point; 
        }

        public CADPoint GetPointAt(int index)
        {
            return _vertices[index].Point;
        }

        public override Bounding Bounding
        {
            //TODO: MKI - refactor respect arc boundigs (center+radius)
            get
            {
                if (_vertices.Count > 0)
                {
                    double minX = double.MaxValue;
                    double minY = double.MaxValue;
                    double maxX = double.MinValue;
                    double maxY = double.MinValue;

                    foreach (CADPoint point in _verticesAsPoints)
                    {
                        minX = point.X < minX ? point.X : minX;
                        minY = point.Y < minY ? point.Y : minY;

                        maxX = point.X > maxX ? point.X : maxX;
                        maxY = point.Y > maxY ? point.Y : maxY;
                    }

                    return new Bounding(new CADPoint(minX, minY), new CADPoint(maxX, maxY));
                }
                else
                {
                    return new Bounding();
                }
            }
        }

        public override object Clone()
        {
            Polyline polyline = base.Clone() as Polyline;
            polyline._vertices.AddRange(_vertices);
            polyline._closed = _closed;

            return polyline;
        }

        protected override DBObject CreateInstance()
        {
            return new Polyline();
        }

        public override void Translate(CADVector translation)
        {
            for (int i = 0; i < this.NumberOfVertices; ++i)
            {
                _vertices[i].Point += translation;
            }
        }

        public override void TransformBy(Matrix3 transform)
        {
            for (int i = 0; i < this.NumberOfVertices; ++i)
            {                
                _vertices[i].Point = transform * _vertices[i].Point;
            }
        }

        public override List<ObjectSnapPoint> GetSnapPoints()
        {
            List<ObjectSnapPoint> snapPnts = new List<ObjectSnapPoint>();
            int numOfVertices = this.NumberOfVertices;
            for (int i = 0; i < numOfVertices; ++i)
            {
                snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.End, GetPointAt(i)));

                if (i != numOfVertices - 1)
                {
                    snapPnts.Add(
                        new ObjectSnapPoint(ObjectSnapMode.Mid, (GetPointAt(i) + GetPointAt(i + 1)) / 2));
                }
            }

            return snapPnts;
        }

        public override List<GripPoint> GetGripPoints()
        {
            List<GripPoint> gripPnts = new List<GripPoint>();
            int numOfVertices = NumberOfVertices;
            for (int i = 0; i < numOfVertices; ++i)
            {
                gripPnts.Add(new GripPoint(GripPointType.End, _verticesAsPoints[i]));
            }
            for (int i = 0; i < numOfVertices - 1; ++i)
            {
                GripPoint midGripPnt = new GripPoint(GripPointType.Mid, (_vertices[i].Point + _vertices[i+1].Point) / 2);
                midGripPnt.xData1 = _vertices[i].Point;
                midGripPnt.xData2 = _vertices[i + 1].Point;
                gripPnts.Add(midGripPnt);
            }
            if (_closed && numOfVertices > 2)
            {
                GripPoint midGripPnt = new GripPoint(GripPointType.Mid, (_vertices[0].Point + _vertices[numOfVertices - 1].Point) / 2);
                midGripPnt.xData1 = _vertices[0];
                midGripPnt.xData2 = _vertices[numOfVertices - 1];
                gripPnts.Add(midGripPnt);
            }

            return gripPnts;
        }

        public override void SetGripPointAt(int index, GripPoint gripPoint, CADPoint newPosition)
        {
            switch (gripPoint.Type)
            {
                case GripPointType.End:
                    {
                        this.SetPointAt(index, newPosition);
                    }
                    break;

                case GripPointType.Mid:
                    {
                        int numOfVertices = NumberOfVertices;
                        int i = index - numOfVertices;
                        if (i >= 0 && i <= numOfVertices-1)
                        {
                            int vIndex1st = i;
                            int vIndex2nd = i + 1;
                            if (vIndex2nd == numOfVertices)
                            {
                                vIndex2nd = 0;
                            }
                            CADVector t = newPosition - gripPoint.Position;
                            this.SetPointAt(vIndex1st, (CADPoint)gripPoint.xData1 + t);
                            this.SetPointAt(vIndex2nd, (CADPoint)gripPoint.xData2 + t);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public override void XmlOut(Filer.XmlFiler filer)
        {
            //TODO: MKI - refactor to use foreach (CADPolyLine2DVertex vertex in _vertices) 
            base.XmlOut(filer);
            
            //
            filer.Write("closed", _closed);
            //
            string strVertices = "";
            int i = 0;
            foreach (CADPoint vertex in _verticesAsPoints)
            {
                if (++i > 1)
                {
                    strVertices += "|";
                }
                strVertices += vertex.X.ToString() + ";" + vertex.Y.ToString();
            }
            filer.Write("vertices", strVertices);
        }

        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            //
            filer.Read("closed", out _closed);
            //
            string strVertices;
            filer.Read("vertices", out strVertices);
            string[] vts = strVertices.Split('|');
            double x = 0;
            double y = 0;
            string[] xy = null;
            foreach (string vtx in vts)
            {
                xy = vtx.Split(';');
                if (xy.Length != 2)
                {
                    continue;
                }
                if (!double.TryParse(xy[0], out x))
                {
                    continue;
                }
                if (!double.TryParse(xy[1], out y))
                {
                    continue;
                }

                _vertices.Add(new CADPolyLine2DVertex(x, y));
            }
        }
    }
}
