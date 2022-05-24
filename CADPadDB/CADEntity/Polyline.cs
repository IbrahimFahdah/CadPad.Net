using System.Collections.Generic;
using CADPadDB.Maths;

namespace CADPadDB.CADEntity
{

    public class Polyline : Entity
    {

        public override string ClassName
        {
            get { return "Polyline"; }
        }

        private List<CADPoint> _vertices = new List<CADPoint>();
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

            gd.Open(Color);
            int numOfVertices = NumberOfVertices;
            for (int i = 0; i < numOfVertices - 1; ++i)
            {
                gd.DrawLine(GetPointAt(i), GetPointAt(i + 1));
            }

            if (closed
                && numOfVertices > 2)
            {
                gd.DrawLine(GetPointAt(numOfVertices - 1), GetPointAt(0));
            }
            gd.Close();
        }

        public void AddVertexAt(int index, CADPoint point)
        {
            _vertices.Insert(index, point);
        }

        public void RemoveVertexAt(int index)
        {
            _vertices.RemoveAt(index);
        }

        public CADPoint GetPointAt(int index)
        {
            return _vertices[index];
        }

        public void SetPointAt(int index, CADPoint point)
        {
            _vertices[index] = point;
        }

        public override Bounding Bounding
        {
            get
            {
                if (_vertices.Count > 0)
                {
                    double minX = double.MaxValue;
                    double minY = double.MaxValue;
                    double maxX = double.MinValue;
                    double maxY = double.MinValue;

                    foreach (CADPoint point in _vertices)
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
                _vertices[i] += translation;
            }
        }

        public override void TransformBy(Matrix3 transform)
        {
            for (int i = 0; i < this.NumberOfVertices; ++i)
            {
                _vertices[i] = transform * _vertices[i];
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
                gripPnts.Add(new GripPoint(GripPointType.End, _vertices[i]));
            }
            for (int i = 0; i < numOfVertices - 1; ++i)
            {
                GripPoint midGripPnt = new GripPoint(GripPointType.Mid, (_vertices[i] + _vertices[i+1]) / 2);
                midGripPnt.xData1 = _vertices[i];
                midGripPnt.xData2 = _vertices[i + 1];
                gripPnts.Add(midGripPnt);
            }
            if (_closed && numOfVertices > 2)
            {
                GripPoint midGripPnt = new GripPoint(GripPointType.Mid, (_vertices[0] + _vertices[numOfVertices - 1]) / 2);
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
            base.XmlOut(filer);

            //
            filer.Write("closed", _closed);
            //
            string strVertices = "";
            int i = 0;
            foreach (CADPoint vertex in _vertices)
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

                _vertices.Add(new CADPoint(x, y));
            }
        }
    }
}
